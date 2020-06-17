using System.Drawing;
using Microsoft.ML.Transforms.Image;
using static FacialExpressionDetector.FacialExpressionDetector;

namespace FacialExpressionDetector
{
    public class ModelInput
    {
        [ImageType(FERPlusOnnxConfig.ImageHeight, FERPlusOnnxConfig.ImageWidth)]
        public Bitmap ImageAsBitmap { get; set; }
    }
}