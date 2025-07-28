using System; // For Math.Clamp
using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public class Ditherer
    {
        private readonly byte[,] matrix;
        private readonly int divisor;
        private readonly int matrixHeight;
        private readonly int matrixWidth;
        private readonly int startingOffset;
        private readonly ProgramTimer timer;

        public long TimeTaken() => timer.Finish();

        public Ditherer()
        {
            timer = new ProgramTimer();
            timer.Start("Dithering image");
            matrix = new byte[,]
            {
                { 0, 0, 7 },
                { 3, 5, 1 },
            };

            divisor = 4; // >>4 = /16
            matrixWidth = matrix.GetUpperBound(1) + 1;
            matrixHeight = matrix.GetUpperBound(0) + 1;

            for (int i = 0; i < matrixWidth; i++)
            {
                if (matrix[0, i] != 0)
                {
                    startingOffset = i - 1;
                    break;
                }
            }
            timer.Stop();
        }

        public void Dither(
            Color[,] data,
            Color original,
            Color transformed,
            int x,
            int y,
            int width,
            int height
        )
        {
            timer.Start();
            // Calculate errors as floats for scaling if needed
            float errorScale = 1.0f; // Reduce to 0.5f if noise is too strong
            int redError = (int)((original.R - transformed.R) * errorScale);
            int greenError = (int)((original.G - transformed.G) * errorScale);
            int blueError = (int)((original.B - transformed.B) * errorScale);

            for (int row = 0; row < matrixHeight; row++)
            {
                int offsetY = y + row;
                for (int col = 0; col < matrixWidth; col++)
                {
                    int coefficient = matrix[row, col];
                    int offsetX = x + (col - startingOffset);

                    if (
                        coefficient != 0
                        && offsetX >= 0
                        && offsetX < width
                        && offsetY >= 0
                        && offsetY < height
                    )
                    {
                        Color offsetPixel = data[offsetX, offsetY]; // Change to data[offsetY, offsetX] for Variant 2 below

                        int newR = (redError * coefficient) >> divisor;
                        int newG = (greenError * coefficient) >> divisor;
                        int newB = (blueError * coefficient) >> divisor;

                        // Clamp to prevent wraparound
                        byte r = (byte)Math.Clamp(offsetPixel.R + newR, 0, 255);
                        byte g = (byte)Math.Clamp(offsetPixel.G + newG, 0, 255);
                        byte b = (byte)Math.Clamp(offsetPixel.B + newB, 0, 255);

                        data[offsetX, offsetY] = new Color(r, g, b); // Change to data[offsetY, offsetX] for Variant 2
                    }
                }
            }
            timer.Stop();
        }
    }
}
