using System.Collections.Generic;

namespace FacialExpressionDetector
{
    public class ModelOutput
    {
        public string Filename { get; set; }

        public List<(string emotion, float probability)> EmotionProbabilities { get; set; }
    }
}