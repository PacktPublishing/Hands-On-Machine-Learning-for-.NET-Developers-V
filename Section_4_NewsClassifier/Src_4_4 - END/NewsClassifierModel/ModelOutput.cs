using System;
using Microsoft.ML.Data;

namespace NewsClassifierModel
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string Category { get; set; }

        public float[] Score { get; set; }
    }
}
