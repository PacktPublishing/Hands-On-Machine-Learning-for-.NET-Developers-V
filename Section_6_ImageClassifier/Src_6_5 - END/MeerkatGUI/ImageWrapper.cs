using System.IO;

namespace MeerkatGUI
{
    public class ImageWrapper
    {
        public string Fullpath { get; set; }
        public string FileName => Path.GetFileName(Fullpath);

        public override string ToString() => FileName;
    }
}