using System;
using System.Diagnostics;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageProcessing
{
    public static class ImageProcessor
    {
        public static void ApplyFilter(Image<Rgba32> image, string filterType)
        {
            Console.WriteLine($"Applying {filterType}...");
            Stopwatch sw = Stopwatch.StartNew();

            switch (filterType.ToLower())
            {
                case "gaussianblur":
                    image.Mutate(ctx => ctx.GaussianBlur(5));
                    break;
                case "sharpen":
                    image.Mutate(ctx => ctx.Contrast(1.5f));
                    break;
                case "brightness":
                    image.Mutate(ctx => ctx.Brightness(2.2f));
                    break;
                case "contrast":
                    image.Mutate(ctx => ctx.Contrast(1.2f));
                    break;
                case "invert":
                    image.Mutate(ctx => ctx.Invert());
                    break;
                default:
                    Console.WriteLine("Invalid filter type. No filter applied.");
                    return;
            }

            sw.Stop();
            Console.WriteLine($"Filter applied in: {sw.ElapsedMilliseconds} ms");
        }

        public static void ResizeImage(Image<Rgba32> image, int newWidth, int newHeight)
        {
            Console.WriteLine($"Resizing image to {newWidth}x{newHeight}...");
            Stopwatch sw = Stopwatch.StartNew();
            image.Mutate(ctx => ctx.Resize(newWidth, newHeight));
            sw.Stop();
            Console.WriteLine($"Resized in: {sw.ElapsedMilliseconds} ms");
        }

        public static void RotateImage(Image<Rgba32> image, float angle)
        {
            Console.WriteLine($"Rotating image by {angle} degrees...");
            Stopwatch sw = Stopwatch.StartNew();
            image.Mutate(ctx => ctx.Rotate(angle));
            sw.Stop();
            Console.WriteLine($"Rotation completed in: {sw.ElapsedMilliseconds} ms");
        }

        public static void SaveImage(Image<Rgba32> image, string outputPath)
        {
            Console.WriteLine($"Saving image to {outputPath}...");
            image.Save(outputPath);
            Console.WriteLine("Image saved successfully.");

            OpenImage(outputPath);
        }

        private static void OpenImage(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                Console.WriteLine("Image opened successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open the image: {ex.Message}");
            }
        }
    }
}