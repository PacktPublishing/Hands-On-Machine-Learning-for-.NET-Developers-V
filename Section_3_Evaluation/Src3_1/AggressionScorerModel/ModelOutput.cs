using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace AggressionScorerModel
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }
    }
}
