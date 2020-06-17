using AggressionScorerModel;
using Microsoft.ML;
using System;
using System.IO;

namespace AggressionScorer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Aggression scorer model builder started!");

            var mlContext = new MLContext(0);

            //Build pipeline
            var inputDataPreparer = mlContext
                                    .Transforms
                                    .Text
                                    .FeaturizeText("Features", "Comment")
                                    .Append(mlContext.Transforms.NormalizeMeanVariance("Features"))
                                    .AppendCacheCheckpoint(mlContext);

            // Create a training algorithm 
            var trainer = mlContext
                            .BinaryClassification
                            .Trainers
                            .LbfgsLogisticRegression();

            var trainingPipeline = inputDataPreparer.Append(trainer);


            //Load data 
            var createdInputFile = @"Data\preparedInput.tsv";
            DataPreparer.CreatePreparedDataFile(createdInputFile, onlySaveSmallSubset: true);

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                path: createdInputFile,
                hasHeader: true,
                separatorChar: '\t',
                allowQuoting: true
            );



            //Fit the model
            Console.WriteLine("Start training model");

            var startTime = DateTime.Now;
            ITransformer model = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine($"Model training finished in {(DateTime.Now - startTime).TotalSeconds} seconds");

            //Test the model
            EvaluateModel(mlContext, model, trainingDataView);


            //Save the model
            Console.WriteLine($"Saving the model");

            if (!Directory.Exists("Model"))
            {
                Directory.CreateDirectory("Model");
            }

            var modelFile = @"Model\\AggressionScoreModel.zip";
            mlContext.Model.Save(model, trainingDataView.Schema, modelFile);

            Console.WriteLine("The model is saved to {0}", modelFile);

        }

        private static void EvaluateModel(MLContext mlContext, ITransformer trainedModel, IDataView testData)
        {
            Console.WriteLine();
            Console.WriteLine("-- Evaluating binary classification model performance --");
            Console.WriteLine();

            var predictedData = trainedModel.Transform(testData);

            var metrics = mlContext.BinaryClassification.Evaluate(predictedData);

            Console.WriteLine($"Accuracy: {metrics.Accuracy:0.###}");

            Console.WriteLine();

            Console.WriteLine("Confusion Matrix");
            Console.WriteLine();
            Console.WriteLine(metrics.ConfusionMatrix.GetFormattedConfusionTable());
            Console.WriteLine();

        }
    }
}
