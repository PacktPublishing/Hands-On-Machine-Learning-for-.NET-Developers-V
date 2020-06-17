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
    }
}
