using AggressionScorerModel;
using Microsoft.ML;
using Microsoft.ML.Calibrators;
using Microsoft.ML.Trainers;
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

            //Load data 
            var createdInputFile = @"Data\preparedInput.tsv";
            DataPreparer.CreatePreparedDataFile(createdInputFile, onlySaveSmallSubset: true);

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                path: createdInputFile,
                hasHeader: true,
                separatorChar: '\t',
                allowQuoting: true
            );

            var inputDataSplit = mlContext.Data.TrainTestSplit(trainingDataView, testFraction: .2, seed: 0);


            //Build pipeline
            var inputDataPreparer = mlContext
                                    .Transforms
                                    .Text
                                    .FeaturizeText("Features", "Comment")
                                    .Append(mlContext.Transforms.NormalizeMeanVariance("Features"))
                                    .AppendCacheCheckpoint(mlContext)
                                    .Fit(inputDataSplit.TrainSet);

            // Create a training algorithm 
            var trainer = mlContext
                            .BinaryClassification
                            .Trainers
                            .LbfgsLogisticRegression();


            

            //Fit the model
            Console.WriteLine("Start training model");

            var startTime = DateTime.Now;
            var transformedData = inputDataPreparer.Transform(inputDataSplit.TrainSet);
            ITransformer model = trainer.Fit(transformedData);

            Console.WriteLine($"Model training finished in {(DateTime.Now - startTime).TotalSeconds} seconds");

            //Test the model
            EvaluateModel(mlContext, model, inputDataPreparer.Transform(inputDataSplit.TestSet));


            //Save the model
            Console.WriteLine($"Saving the model");

            if (!Directory.Exists("Model"))
            {
                Directory.CreateDirectory("Model");
            }

            var modelFile = @"Model\\AggressionScoreModel.zip";
            mlContext.Model.Save(model, trainingDataView.Schema, modelFile);

            Console.WriteLine("The model is saved to {0}", modelFile);

            var dataPreparePipelineFile = "dataPreparePipeline.zip";
            Console.WriteLine($"Saving the input data preparing pipeline");

            mlContext.Model.Save(inputDataPreparer, trainingDataView.Schema, dataPreparePipelineFile);

            Console.WriteLine("The pipeline is saved to {0}", dataPreparePipelineFile);

            var retrainedModel = RetrainModel(modelFile, dataPreparePipelineFile);

            var completeRetrainedPipeline = inputDataPreparer.Append(retrainedModel);

            Console.WriteLine($"Saving the retrained model");
            string retrainedModelFile = @"Model\\AggressionScoreRetrainedModel.zip";

            mlContext.Model.Save(completeRetrainedPipeline, trainingDataView.Schema, retrainedModelFile);

            Console.WriteLine("The model is saved to {0}", retrainedModelFile);

            EvaluateModel(mlContext, completeRetrainedPipeline, inputDataSplit.TestSet);

        }

        private static ITransformer RetrainModel(string modelFile, string dataPreparationPipelineFile)
        {
            MLContext mlContext = new MLContext(0);

            // Load pre trained model
            ITransformer pretrainedModel = mlContext.Model.Load(modelFile, out _);

            // Extract pretrained model parameters
            var pretrainedModelParameters =
                ((ISingleFeaturePredictionTransformer
                    <CalibratedModelParametersBase<LinearBinaryModelParameters, PlattCalibrator>>)
                    pretrainedModel)
                .Model.SubModel;

            string dataFile = @"Data\preparedInput.tsv";
            DataPreparer.CreatePreparedDataFile(dataFile, onlySaveSmallSubset: false);

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                path: dataFile,
                hasHeader: true,
                separatorChar: '\t',
                allowQuoting: true
            );

            // Load data preparation pipeline
            ITransformer dataPrepPipeline = mlContext.Model.Load(dataPreparationPipelineFile, out _);

            // Prepare input data to a form consumable by a machine learning model
            var newData = dataPrepPipeline.Transform(trainingDataView);

            // Retrain model
            Console.WriteLine("Start retraining model");

            var startTime = DateTime.Now;

            var retrainedModel =
                mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression()
                    .Fit(newData, pretrainedModelParameters);

            Console.WriteLine($"Model retraining finished in {(DateTime.Now - startTime).TotalSeconds} seconds");


            return retrainedModel;

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
