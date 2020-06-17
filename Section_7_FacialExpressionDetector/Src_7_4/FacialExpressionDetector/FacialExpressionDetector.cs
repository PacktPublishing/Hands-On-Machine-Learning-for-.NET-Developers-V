using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FacialExpressionDetector
{
    /// <summary>
    /// Facial expression emotion scorer with the FER+ ONNX model
    /// https://github.com/onnx/models/tree/master/vision/body_analysis/emotion_ferplus
    /// </summary>
    public class FacialExpressionDetector
    {
        public struct FERPlusOnnxConfig
        {
            public const string ModelFilePath = "Model\\emotion-ferplus-8.onnx";

            public const int ImageWidth = 64;
            public const int ImageHeight = 64;

            public const string InputLayer = "Input3";
            public const string OutputLayer = "Plus692_Output_0";

            public static readonly string[] Labels = { "neutral",
                "happiness",
                "surprise",
                "sadness",
                "anger",
                "disgust",
                "fear",
                "contempt" };

           public static float[] Softmax(float[] values)
            {
                var exponentialValues = values.Select(v => Math.Exp(v - values.Max()))
                    .ToArray();

                return exponentialValues.Select(exp => (float)(exp / exponentialValues.Sum()))
                    .ToArray();
            }
        }

        public ModelOutput DetectEmotionInBitmap(Bitmap image)
        {
            //TODO: Use ONNX model to score emotion in image
            float[] scoredImage = Enumerable.Repeat(0f,8).ToArray();


            //Format the score with labels
            return WrapModelOutput(scoredImage);
        }

        public IEnumerable<ModelOutput> DetectEmotionsInImageFiles(string[] imagePaths)
        {

            //TODO: Use ONNX model to score emotion in Bitmap
            float[][] scoredImages = Enumerable.Repeat(
                                                 Enumerable.Repeat(0f, 8).ToArray(),
                                                 imagePaths.Length
                                                 )
                                               .ToArray(); 


            //Format the score with labels
            var result =
                imagePaths.Select(Path.GetFileName)
                    .Zip(
                        scoredImages,
                        (fileName, probabilities) =>  WrapModelOutput(probabilities,fileName)
                    );


            return result;
        }

        private static ModelOutput WrapModelOutput(float[] probabilities, string filename = null)
        {
            List<(string emotion, float probability)> mergedLabelsWithProbabilities =
                                                          FERPlusOnnxConfig
                                                          .Labels
                                                          .Zip(
                                                                probabilities,
                                                                (emotion, probability) => (emotion, probability))
                                                          .ToList();
            
            return new ModelOutput()
            {
                Filename = filename,
                EmotionProbabilities = mergedLabelsWithProbabilities
            };
        }

        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public FacialExpressionDetector()
        {
            _mlContext = new MLContext();
            _model = LoadModel();
        }

        private ITransformer LoadModel()
        {
            var onnxScorer = _mlContext
                .Transforms
                .ApplyOnnxModel(
                    modelFile: FERPlusOnnxConfig.ModelFilePath,
                    inputColumnNames: new[] { FERPlusOnnxConfig.InputLayer },
                    outputColumnNames: new[] { FERPlusOnnxConfig.OutputLayer }

                );

            var preProcessingPipeline = _mlContext
                .Transforms
                .ConvertToGrayscale(
                    inputColumnName: nameof(ModelInput.ImageAsBitmap),
                    outputColumnName: nameof(ModelInput.ImageAsBitmap))
                 .Append(_mlContext
                    .Transforms
                    .ResizeImages(
                        inputColumnName: nameof(ModelInput.ImageAsBitmap),
                        imageWidth: FERPlusOnnxConfig.ImageWidth,
                        imageHeight: FERPlusOnnxConfig.ImageHeight,
                        outputColumnName: nameof(ModelInput.ImageAsBitmap)
                    )).Append(_mlContext
                    .Transforms
                    .ExtractPixels(
                        inputColumnName: nameof(ModelInput.ImageAsBitmap),
                        outputColumnName: FERPlusOnnxConfig.InputLayer,
                        outputAsFloatArray: true,
                        colorsToExtract: ImagePixelExtractingEstimator.ColorBits.Red
                    )); //Note that the result fits the float[1,1,64,64] input node type


            var completePipeline = preProcessingPipeline.Append(onnxScorer);

            // Fit scoring pipeline to the ModelInput structure to create a model
            var emptyInput = _mlContext.Data.LoadFromEnumerable(new List<ModelInput>());
            var model = completePipeline.Fit(emptyInput);

            return model;
        }

        private float[][] ScoreImageList(List<ModelInput> imageInputs)
        {
            // Create an IDataView from the image list
            IDataView imageDataView = _mlContext.Data.LoadFromEnumerable(imageInputs);

            // Transform the IDataView with the model
            IDataView scoredData = _model.Transform(imageDataView);

            // Extract the scores from the output layer
            var scoringValues = scoredData.GetColumn<float[]>(FERPlusOnnxConfig.OutputLayer);

            // Run the scores through the SoftMax function
            float[][] probabilities;
            try
            {
                probabilities = scoringValues.Select(FERPlusOnnxConfig.Softmax)
                                         .ToArray();
            }
            catch
            {
                probabilities = Enumerable.Repeat(
                                                    Enumerable.Repeat(0f, 8).ToArray(), 1)
                                          .ToArray();
            }

            return probabilities;
        }
    }
}
