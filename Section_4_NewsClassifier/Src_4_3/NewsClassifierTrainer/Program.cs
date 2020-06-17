using Common;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using NewsClassifierModel;
using System;

namespace NewsClassifierTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("News classification trainer started");

            FindTheBestModel();

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
    }
}
