using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    public class Day20 : Day<int>
    {
        private readonly string EnhancementAlgorithm;
        private Dictionary<(int col, int row), char> Image;

        public Day20() : base(2021, "day20")
        {
            EnhancementAlgorithm = Input.First();
            var image = Input.Skip(2).Select(row => row.ToCharArray()).ToArray();

            Image = new();
            foreach (var col in Enumerable.Range(0, image.Length))
            {
                foreach (var row in Enumerable.Range(0, image[0].Length))
                {
                    Image.Add((col, row), image[col][row]);
                }
            }
        }

        public override int PartOne()
        {
            var enhancedImage = new Dictionary<(int col, int row), char>(Image);
            for (int step = 0; step < 2; step++)
            {
                enhancedImage = Enhance(enhancedImage, step % 2 == 0 ? '.' : '#');
            }
            return enhancedImage.Count(pixel => pixel.Value == '#');
        }

        public override int PartTwo()
        {
            var enhancedImage = new Dictionary<(int col, int row), char>(Image);
            for (int step = 0; step < 50; step++)
            {
                enhancedImage = Enhance(enhancedImage, step % 2 == 0 ? '.' : '#');
            }
            return enhancedImage.Count(pixel => pixel.Value == '#');
        }

        private Dictionary<(int col, int row), char> Enhance(Dictionary<(int col, int row), char> image, char pad)
        {
            var output = new Dictionary<(int col, int row), char>(image);

            var xMin = image.Min(pixel => pixel.Key.row) - 1;
            var xMax = image.Max(pixel => pixel.Key.row) + 1;
            var yMin = image.Min(pixel => pixel.Key.col) - 1;
            var yMax = image.Max(pixel => pixel.Key.col) + 1;

            foreach (var pixel in Enumerable.Range(yMin, yMax - yMin + 1)
                .SelectMany(col => Enumerable.Range(xMin, xMax - xMin + 1), (col, row) => (col, row)))
            {
                output[pixel] = EnhancePixel(image, pixel, pad);
            }

            return output;
        }

        private char EnhancePixel(Dictionary<(int col, int row), char> image, (int col, int row) pixel, char defaultValue)
        {
            var binary = string.Empty;
            foreach (var col in Enumerable.Range(pixel.col - 1, 3))
            {
                foreach (var row in Enumerable.Range(pixel.row - 1, 3))
                {
                    binary += image.GetValueOrDefault((col, row), defaultValue) == '#' ? "1" : "0";
                }
            }

            return EnhancementAlgorithm[Convert.ToInt32(binary, 2)];
        }
    }
}
