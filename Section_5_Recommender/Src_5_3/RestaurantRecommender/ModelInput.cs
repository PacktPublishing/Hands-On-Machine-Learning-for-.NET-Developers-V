using Microsoft.ML.Data;

namespace RestaurantRecommender
{
    //UserId	RestaurantName	TotalRating

    public class ModelInput
    {
        [LoadColumn(0)]
        public string UserId { get; set; }

        [LoadColumn(1)]
        public string RestaurantName { get; set; }

        [LoadColumn(2)]
        public float TotalRating { get; set; }
    }
}