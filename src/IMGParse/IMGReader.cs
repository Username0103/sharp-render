using sharp_render.src.Common;

// format info: https://www.ece.ualberta.ca/~elliott/ee552/studentAppNotes/2003_w/misc/bmp_file_format/bmp_file_format.htm

namespace sharp_render.src.IMGParse
{
    public static class ImgReader
    {
        public static Color[] Extract1D(BMPFile file)
        {
            IEnumerable<Color> Colors = [];
            IEnumerable<string[]> Pixels = file.IMGHx.Chunk(file.Info.BitsPerPixel / 8);
            foreach (string[] Pixel in Pixels)
            {
                if (Pixel.Length == 3)
                {
                    // this assumes bpp is 24
                    Colors = Colors.Append(
                        new Color(
                            // blue, green, red

                            Utils.HxToInt([Pixel[2]]),
                            Utils.HxToInt([Pixel[1]]),
                            Utils.HxToInt([Pixel[0]])
                        )
                    );
                }
            }
            return [.. Colors];
        }

        public static Color[,] To2D(Color[] Colors, int Height, int Width)
        {
            Color[,] ColorMatrix = new Color[Height, Width];
            if (Colors.Length < Height * Width)
                throw new ArgumentException(
                    $"Size mismatch: Pixel data expected: {Height * Width}; Got: {Colors.Length}"
                );
            int count = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    ColorMatrix[i, j] = Colors[count];
                    count++;
                }
            }
            return Utils.ReverseMatrix(ColorMatrix);
        }
    }
}
