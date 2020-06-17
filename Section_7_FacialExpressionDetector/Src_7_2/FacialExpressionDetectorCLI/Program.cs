using System;
using System.IO;

namespace FacialExpressionDetectorCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Image facial expression detector - using FER+ ONNX model");
            Console.WriteLine();

            // Load all image paths
            var imageFolder = GetImageFolderFromArgs(args);
            

            // Run the images through the ONNX model


            // Print the results


        }

        private static string GetImageFolderFromArgs(string[] args)
        {
            //Default to "images" folder if no arguments are given
            if (args.Length != 1) return "images"; 

            if (Directory.Exists(args[0])) return args[0];
            
            Console.WriteLine("Given image directory does not exist");
            Environment.Exit(1);
            
            return null;

        }
    }
}
