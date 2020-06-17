using Microsoft.ML.Data;

namespace NewsClassifierModel
{
    //uci-news-aggregator.csv
    //ID,TITLE,URL,PUBLISHER,CATEGORY,STORY,HOSTNAME,TIMESTAMP

    public class ModelInput
    {
        [LoadColumn(1)]
        public string Title { get; set; }

        [LoadColumn(4)]
        public string Category { get; set; }
    }
}