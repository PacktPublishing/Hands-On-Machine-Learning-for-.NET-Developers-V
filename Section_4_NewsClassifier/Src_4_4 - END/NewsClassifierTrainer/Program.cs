using Common;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using NewsClassifierModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NewsClassifierTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("News classification trainer started");

            //FindTheBestModel();

            TrainTheModel();
            TrainTheModelWithUnbalancedData();
        }
        private static void TrainTheModelWithUnbalancedData()
        {
            Console.WriteLine("The News classifier model trainer");

            var mlContext = new MLContext(seed: 0);

            var trainDataPath = "Data\\uci-news-aggregator.csv";

            var unbalancedDataFile = "Data\\unbalanced.csv";
            CreateUnbalancedDataFile(trainDataPath, unbalancedDataFile);



            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                unbalancedDataFile,
                hasHeader: true,
                separatorChar: ',',
                allowQuoting: true);

            var preProcessingPipeline = mlContext.Transforms.Conversion
                .MapValueToKey(inputColumnName: "Category", outputColumnName: "Label")
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Title",
                    outputColumnName: "Features"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .AppendCacheCheckpoint(mlContext);

            var trainer = mlContext
                    .MulticlassClassification
                    .Trainers
                    .OneVersusAll(mlContext.BinaryClassification.Trainers.AveragedPerceptron());

            var trainingPipeline = preProcessingPipeline
                                    .Append(trainer)
                                    .Append(
                                        mlContext
                                                .Transforms
                                                .Conversion
                                                .MapKeyToValue("PredictedLabel")
                                           );
            Console.WriteLine("Cross validating model");
            var cvResults = mlContext
                                .MulticlassClassification
                                .CrossValidate(trainingDataView, trainingPipeline);

            var microAccuracy = cvResults.Average(m => m.Metrics.MicroAccuracy);
            var macroAccuracy = cvResults.Average(m => m.Metrics.MacroAccuracy);
            var logLossReduction = cvResults.Average(m => m.Metrics.LogLossReduction);

            Console.WriteLine();
            Console.WriteLine($"Cross validation Metrics for our model");
            Console.WriteLine($"--------------------------------------");
            Console.WriteLine($" MicroAccuracy:    {microAccuracy:0.###}");
            Console.WriteLine($" MacroAccuracy:    {macroAccuracy:0.###}");
            Console.WriteLine($" LogLossReduction: {logLossReduction:#.###}");
            Console.WriteLine($"--------------------------------------");
            Console.WriteLine();

            //Train final model on all data
            Console.WriteLine("Training model...");
            var startingTime = DateTime.Now;

            var finalModel = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine($"Model training finished in {(DateTime.Now - startingTime).TotalSeconds} seconds");
            Console.WriteLine();

            //Save model
            Console.WriteLine("Saving model");

            if (!Directory.Exists("Model"))
            {
                Directory.CreateDirectory("Model");
            }

            var modelPath = "Model\\NewsClassificationModel.zip";
            mlContext.Model.Save(finalModel, trainingDataView.Schema, modelPath);

            Console.WriteLine($"Model saved to {modelPath}");

        }

        private static void TrainTheModel()
        {
            Console.WriteLine("The News classifier model trainer");

            var mlContext = new MLContext(seed: 0);

            var trainDataPath = "Data\\uci-news-aggregator.csv";
            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                trainDataPath,
                hasHeader: true,
                separatorChar: ',',
                allowQuoting: true);

            var preProcessingPipeline = mlContext.Transforms.Conversion
                .MapValueToKey(inputColumnName: "Category", outputColumnName: "Label")
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Title",
                    outputColumnName: "Features"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .AppendCacheCheckpoint(mlContext);

            var trainer = mlContext
                    .MulticlassClassification
                    .Trainers
                    .OneVersusAll(mlContext.BinaryClassification.Trainers.AveragedPerceptron());

            var trainingPipeline = preProcessingPipeline
                                    .Append(trainer)
                                    .Append(
                                        mlContext
                                                .Transforms
                                                .Conversion
                                                .MapKeyToValue("PredictedLabel")
                                           );
            Console.WriteLine("Cross validating model");
            var cvResults = mlContext
                                .MulticlassClassification
                                .CrossValidate(trainingDataView, trainingPipeline);

            var microAccuracy = cvResults.Average(m => m.Metrics.MicroAccuracy);
            var macroAccuracy = cvResults.Average(m => m.Metrics.MacroAccuracy);
            var logLossReduction = cvResults.Average(m => m.Metrics.LogLossReduction);

            Console.WriteLine();
            Console.WriteLine($"Cross validation Metrics for our model");
            Console.WriteLine($"--------------------------------------");
            Console.WriteLine($" MicroAccuracy:    {microAccuracy:0.###}");
            Console.WriteLine($" MacroAccuracy:    {macroAccuracy:0.###}");
            Console.WriteLine($" LogLossReduction: {logLossReduction:#.###}");
            Console.WriteLine($"--------------------------------------");
            Console.WriteLine();

            //Train final model on all data
            Console.WriteLine("Training model...");
            var startingTime = DateTime.Now;

            var finalModel = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine($"Model training finished in {(DateTime.Now - startingTime).TotalSeconds} seconds");
            Console.WriteLine();

            //Save model
            Console.WriteLine("Saving model");

            if (!Directory.Exists("Model"))
            {
                Directory.CreateDirectory("Model");
            }

            var modelPath = "Model\\NewsClassificationModel.zip";
            mlContext.Model.Save(finalModel, trainingDataView.Schema, modelPath);

            Console.WriteLine($"Model saved to {modelPath}");

        }


        private static void FindTheBestModel()
        {

            Console.WriteLine("Finding the best model using AutoML");

            var mlContext = new MLContext(seed: 0);

            var trainingDataPath = "Data\\uci-news-aggregator.csv";

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                trainingDataPath,
                hasHeader: true,
                separatorChar: ',',
                allowQuoting: true);

            var preProcessingPipeline = mlContext.Transforms.Conversion
                .MapValueToKey(inputColumnName: "Category", outputColumnName: "Category");

            var mappedInputData = preProcessingPipeline.Fit(trainingDataView).Transform(trainingDataView);

            var experimentSettings = new MulticlassExperimentSettings
            {
                MaxExperimentTimeInSeconds = 300,
                CacheBeforeTrainer = CacheBeforeTrainer.On,
                OptimizingMetric = MulticlassClassificationMetric.MicroAccuracy,
                CacheDirectory = null
            };

            var experiment =
                mlContext.Auto().CreateMulticlassClassificationExperiment(experimentSettings);

            Console.WriteLine("Starting experiments");

            var experimentResult =
                        experiment.Execute(
                            trainData: mappedInputData,
                            labelColumnName: "Category",
                            progressHandler: new MulticlassExperimentProgressHandler()
                );

            Console.WriteLine("Metrics from best run:");

            var metrics = experimentResult.BestRun.ValidationMetrics;

            Console.WriteLine($"MicroAccuracy: {metrics.MicroAccuracy:0.##}");
            Console.WriteLine($"MacroAccuracy: {metrics.MacroAccuracy:0.##}");


        }

        private static void CreateUnbalancedDataFile(string inputDataFile, string outputDataFile)
        {
            var inputFileRows = File.ReadAllLines(inputDataFile);

            var outputRows = new List<string>();


            //Add header to output
            outputRows.Add(inputFileRows.First());

            int entertainmentSamples = 0;
            int businessSamples = 0;
            int technologySamples = 0;
            int medicineSamples = 0;


            var randomGenerator = new Random(0);

            foreach (var row in inputFileRows.Skip(1))
            {
                if (row.Contains(",b,")) //business sample
                {
                    //Only add it 10% of the times
                    if (randomGenerator.NextDouble() <= .1)
                    {
                        outputRows.Add(row);
                        businessSamples++;
                    }
                }
                else if (row.Contains(",e,")) //entertainment sample
                {
                    //Only add it 10% of the times
                    if (randomGenerator.NextDouble() <= .1)
                    {
                        outputRows.Add(row);
                        entertainmentSamples++;
                    }
                }
                else if (row.Contains(",t,")) //technology sample
                {
                    outputRows.Add(row);
                    technologySamples++;
                }
                else if (row.Contains(",m,")) //medicine sample
                {
                    outputRows.Add(row);
                    medicineSamples++;
                }
            }

            File.WriteAllLines(outputDataFile, outputRows);

            Console.WriteLine();
            Console.WriteLine($"Samples in the training data");
            Console.WriteLine($"----------------------------");
            Console.WriteLine($"Business: {businessSamples}");
            Console.WriteLine($"Entertainment: {entertainmentSamples}");
            Console.WriteLine($"Technology: {technologySamples}");
            Console.WriteLine($"Medicine: {medicineSamples}");
            Console.WriteLine($"----------------------------");
            Console.WriteLine();

        }
    }
}
