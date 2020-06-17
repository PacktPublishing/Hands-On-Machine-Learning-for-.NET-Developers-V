using MeerkatModel;
using Microsoft.ML;
using Microsoft.ML.Vision;
using System;
using System.IO;
using System.Linq;

namespace ImageClassifierTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Meerkat image classification trainer");
            Console.WriteLine();

            // We don't want to copy the images to the bin folder, so we need to 
            // specify the full path to the folders in the project
            var imagesPath = Path.Combine(AppContext.BaseDirectory,
                                            @"..\..\..\images");

            // A folder that will contain intermediate results
            var workspacePath = Path.Combine(AppContext.BaseDirectory,
                                                @"..\..\..\workspace");
            Directory.CreateDirectory(workspacePath);


            var mlContext = new MLContext(0);

            // Set up the data pre processing pipeline
            var preprocessingPipeline =
                                     mlContext
                                            .Transforms
                                            .LoadRawImageBytes(
                                                outputColumnName: "ImageBytes",
                                                imageFolder: imagesPath,
                                                inputColumnName: "ImagePath")
                                        .Append(
                                        mlContext
                                                .Transforms
                                                .Conversion
                                                .MapValueToKey(
                                                    inputColumnName: "Label",
                                                    outputColumnName: "LabelAsKey"
                                                 )
                                               );

            //Load the training data
            var imageFilePaths = Directory.GetFiles(imagesPath,
                "*.jpg",
                searchOption: SearchOption.AllDirectories);

            // Create the ModelInput DataView instead of loading from csv
            var labeledImagesPaths = imageFilePaths
                .Select(i => new ModelInput()
                {
                    Label = Directory.GetParent(i).Name,
                    ImagePath = i
                });

            IDataView allImagesDataView = mlContext
                                            .Data
                                            .LoadFromEnumerable(labeledImagesPaths);

            IDataView shuffledImageDataView = mlContext
                                                .Data
                                                .ShuffleRows(allImagesDataView, 0);

            Console.WriteLine("Pre processing images....");
            var timestamp = DateTime.Now;

            // Pre Process images and split into train/test/validation
            IDataView preProcessedImageDataView = preprocessingPipeline
                .Fit(shuffledImageDataView)
                .Transform(shuffledImageDataView);

            Console.WriteLine($"Image preprocessing done in {(DateTime.Now - timestamp).TotalSeconds} seconds");
            Console.WriteLine();

            var firstSplit = mlContext
                               .Data
                               .TrainTestSplit(data: preProcessedImageDataView,
                                               testFraction: 0.3,
                                               seed: 0);
            var trainSet = firstSplit.TrainSet;

            var secondSplit = mlContext
                                       .Data
                                       .TrainTestSplit(data: firstSplit.TestSet,
                                                       testFraction: 0.5, seed: 0);

            var validationSet = secondSplit.TrainSet;
            var testSet = secondSplit.TestSet;

            // Set up trainer
            var classifierOptions = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = "ImageBytes",
                LabelColumnName = "LabelAsKey",
                Arch = ImageClassificationTrainer.Architecture.InceptionV3,

                TestOnTrainSet = false,
                ValidationSet = validationSet,

                ReuseTrainSetBottleneckCachedValues = true,
                ReuseValidationSetBottleneckCachedValues = true,
                WorkspacePath = workspacePath,

                MetricsCallback = Console.WriteLine
            };

            var trainingPipeline = mlContext
                                    .MulticlassClassification
                                    .Trainers
                                    .ImageClassification(classifierOptions)
                                    .Append(mlContext
                                                .Transforms
                                                .Conversion
                                                .MapKeyToValue("PredictedLabel"));

            Console.WriteLine("Training model....");
            timestamp = DateTime.Now;

            var trainedModel = trainingPipeline.Fit(trainSet);

            Console.WriteLine($"Model training done in {(DateTime.Now - timestamp).TotalSeconds} seconds");
            Console.WriteLine();

            Console.WriteLine("Calculating metrics...");

            IDataView evaluationData = trainedModel.Transform(testSet);
            var metrics = mlContext
                            .MulticlassClassification
                            .Evaluate(evaluationData, "LabelAsKey");

            Console.WriteLine($"LogLoss: {metrics.LogLoss}");
            Console.WriteLine($"LogLossReduction: {metrics.LogLossReduction}");
            Console.WriteLine($"MicroAccuracy: {metrics.MicroAccuracy}");
            Console.WriteLine($"MacroAccuracy: {metrics.MacroAccuracy}");
            Console.WriteLine();
            Console.WriteLine($"{metrics.ConfusionMatrix.GetFormattedConfusionTable()}");

            Console.WriteLine();
            Console.WriteLine("Saving model");

            Directory.CreateDirectory("Model");
            mlContext.Model.Save(trainedModel, preProcessedImageDataView.Schema, "Model\\trainedModel.zip");

            Console.WriteLine();
        }
    }
}
