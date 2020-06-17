using System.IO;
using System.Linq;

namespace RestaurantRecommender
{
    public class DataPreparer
    {
        /// <summary>
        /// Merges the source data into a single training data file
        /// </summary>
        /// <param name="trainingDataFile"></param>
        public static void PreprocessData(string trainingDataFile)
        {

            if (File.Exists(trainingDataFile))
            {
                return;
            }

            // Load all restaurants with ID into a dictionary
            var restaurantIdMap = File.ReadAllLines("Data\\geoplaces2.csv")
                .Skip(1)
                .Select(l => new
                {
                    restaurantId = int.Parse(l.Split(',')[0]),
                    restaurantName = l.Split(',')[4]
                })
                .ToDictionary(arg => arg.restaurantId, arg => arg.restaurantName);

            // Load all ratings for the restaurant IDs, sum into a total rating per restaurant and map the ID to a restaurant name
            var processedOutput = File.ReadAllLines("Data\\rating_final.csv")
                .Skip(1).Select(r =>
                {
                    var rowParts = r.Split(',');

                    var userId = rowParts[0];
                    var restaurantName = restaurantIdMap[int.Parse(rowParts[1])];
                    var totalRating = int.Parse(rowParts[2]) + int.Parse(rowParts[3]) + int.Parse(rowParts[4]);

                    return $"{userId}\t{restaurantName}\t{totalRating}";
                }).ToList();

            //Insert header
            processedOutput.Insert(0, "UserId\tRestaurantName\tTotalRating");

            File.WriteAllLines(trainingDataFile, processedOutput);
        }
    }

}