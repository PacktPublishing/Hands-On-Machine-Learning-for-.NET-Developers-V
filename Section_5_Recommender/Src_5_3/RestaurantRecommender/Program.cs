using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantRecommender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Restaurants recommender");

            MLContext mlContext = new MLContext(0);

            var trainingDataFile = "Data\\trainingData.tsv";
            DataPreparer.PreprocessData(trainingDataFile);

            IDataView trainingDataView = mlContext
                                           .Data
                                           .LoadFromTextFile<ModelInput>
                                           (trainingDataFile, hasHeader: true);

            var dataProcessingPipeline =
                mlContext
                    .Transforms
                    .Conversion
                    .MapValueToKey(outputColumnName: "UserIdEncoded",
                                    inputColumnName: nameof(ModelInput.UserId))
                .Append(mlContext
                        .Transforms
                        .Conversion
                        .MapValueToKey(outputColumnName: "RestaurantNameEncoded",
                                        inputColumnName: nameof(ModelInput.RestaurantName)));

            var finalOptions = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "UserIdEncoded",
                MatrixRowIndexColumnName = "RestaurantNameEncoded",
                LabelColumnName = "TotalRating",
                NumberOfIterations = 10,
                ApproximationRank = 200,
                Quiet = true
            };

            var trainer = mlContext.Recommendation().Trainers.MatrixFactorization(finalOptions);
            var trainingPipeLine = dataProcessingPipeline.Append(trainer);

            Console.WriteLine("Training model");
            var model = trainingPipeLine.Fit(trainingDataView);

            // View results
            var testUserId = "U1134";

            var predictionEngine = mlContext
                .Model
                .CreatePredictionEngine<ModelInput, ModelOutput>(model);

            var alreadyRatedRestaurants =
                mlContext
                    .Data
                    .CreateEnumerable<ModelInput>(trainingDataView, false)
                    .Where(i => i.UserId == testUserId)
                    .Select(r => r.RestaurantName)
                    .Distinct();

            var allRestaurantNames = trainingDataView
               .GetColumn<string>("RestaurantName")
               .Distinct()
               .Where(r => !alreadyRatedRestaurants.Contains(r));

            var scoredRestaurants =
                    allRestaurantNames.Select(restName =>
                    {
                        var prediction = predictionEngine.Predict(
                                                new ModelInput()
                                                {
                                                    RestaurantName = restName,
                                                    UserId = testUserId
                                                });

                        return (RestaurantName: restName, PredictedRating: prediction.Score);
                    });

            var top10Restaurants = scoredRestaurants
                                    .OrderByDescending(s => s.PredictedRating)
                                    .Take(10);

            Console.WriteLine();
            Console.WriteLine($"Top 10 restaurants for {testUserId}");
            Console.WriteLine($"----------------------------");

            foreach (var input in top10Restaurants)
            {
                Console.WriteLine($"Predicted rating [{input.PredictedRating:#.0}] for restaurant: '{input.RestaurantName}'");
            }
        }
    }
}
