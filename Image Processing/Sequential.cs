using System;
using System.Diagnostics;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageProcessing
{
    class Sequential
    {
        static void Main()
        {
            string projectDir = AppDomain.CurrentDomain.BaseDirectory;
            string inputDir = Path.Combine(projectDir, "inputs");
            string outputDir = Path.Combine(projectDir, "outputs");

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            Console.Write("Enter input image filename (e.g., testimage1.jpg): ");
            string inputFilename = Console.ReadLine();
            string inputPath = Path.Combine(inputDir, inputFilename);

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Error: Input file '{inputPath}' not found.");
                return;
            }

            Console.Write("Enter output image filename (e.g., output1.jpg): ");
            string outputFilename = Console.ReadLine();
            string outputPath = Path.Combine(outputDir, outputFilename);

            Console.Write("Enter new width: ");
            int newWidth = int.Parse(Console.ReadLine());

            Console.Write("Enter new height: ");
            int newHeight = int.Parse(Console.ReadLine());

            Console.Write("Enter rotation angle: ");
            float rotationAngle = float.Parse(Console.ReadLine());

            Console.Write("Enter filter type (gaussianblur, sharpen, brightness, contrast, invert): ");
            string filterType = Console.ReadLine();

            Stopwatch totalStopwatch = Stopwatch.StartNew();

            using (var image = Image.Load<Rgba32>(inputPath))
            {
                ImageProcessor.ApplyFilter(image, filterType);
                ImageProcessor.ResizeImage(image, newWidth, newHeight);
                ImageProcessor.RotateImage(image, rotationAngle);
                ImageProcessor.SaveImage(image, outputPath);
            }

            totalStopwatch.Stop();
            Console.WriteLine($"Total processing time: {totalStopwatch.ElapsedMilliseconds} ms");

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}