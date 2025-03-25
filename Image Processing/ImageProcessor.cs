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

        public static void DownscaleImage(Image<Rgba32> image, int targetWidth, int targetHeight)
        {
            Console.WriteLine($"Downscaling image to {targetWidth}x{targetHeight}...");
            Stopwatch sw = Stopwatch.StartNew();

            image.Mutate(ctx => ctx.Resize(targetWidth, targetHeight));

            sw.Stop();
            Console.WriteLine($"Downscaled in: {sw.ElapsedMilliseconds} ms");
        }

        public static void ResizeImageInTilesAndSave(Image<Rgba32> image, int newWidth, int newHeight, string outputPath, int tileSize = 100)
        {
            Console.WriteLine($"Resizing image to {newWidth}x{newHeight} using smaller tiles and saving intermediate results...");

            int originalWidth = image.Width;
            int originalHeight = image.Height;

            Stopwatch sw = Stopwatch.StartNew();

            int xTiles = (originalWidth + tileSize - 1) / tileSize;
            int yTiles = (originalHeight + tileSize - 1) / tileSize;

            for (int x = 0; x < xTiles; x++)
            {
                for (int y = 0; y < yTiles; y++)
                {
                    int tileX = x * tileSize;
                    int tileY = y * tileSize;
                    int tileWidth = Math.Min(tileSize, originalWidth - tileX);
                    int tileHeight = Math.Min(tileSize, originalHeight - tileY);

                    var tile = image.Clone(ctx => ctx.Crop(new Rectangle(tileX, tileY, tileWidth, tileHeight)));

                    var resizedTile = tile.Clone(ctx => ctx.Resize(newWidth / xTiles, newHeight / yTiles));

                    string tempTilePath = Path.Combine(Path.GetDirectoryName(outputPath), $"temp_{x}_{y}.jpg");
                    resizedTile.Save(tempTilePath);

                    tile.Dispose();
                    resizedTile.Dispose();

                }
            }

            sw.Stop();
            Console.WriteLine($"Resizing with tiles completed in: {sw.ElapsedMilliseconds} ms");

            CombineTilesToFinalImage(xTiles, yTiles, outputPath);
        }

        private static void CombineTilesToFinalImage(int xTiles, int yTiles, string outputPath)
        {
            Console.WriteLine("Combining tiles into the final image...");
            var finalImage = new Image<Rgba32>(xTiles * 100, yTiles * 100); 

            for (int x = 0; x < xTiles; x++)
            {
                for (int y = 0; y < yTiles; y++)
                {
                    string tempTilePath = Path.Combine(Path.GetDirectoryName(outputPath), $"temp_{x}_{y}.jpg");
                    var tile = Image.Load<Rgba32>(tempTilePath);

                    finalImage.Mutate(ctx => ctx.DrawImage(tile, new Point(x * 100, y * 100), 1f));

                    File.Delete(tempTilePath);
                }
            }

            finalImage.Save(outputPath);
            Console.WriteLine($"Final image saved to {outputPath}");
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
