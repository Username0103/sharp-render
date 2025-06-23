namespace sharp_render.src.Common
{
    public class IMGPrint
    {
        public static void Print(Color[,] image, Dictionary<Color, int> color2Ansi)
        {
            foreach (int x in Enumerable.Range(0, image.GetLength(0)))
            {
                Console.WriteLine();
                foreach (int y in Enumerable.Range(0, image.GetLength(1)))
                {
                    Color pixel = image[x, y];
                    int code = color2Ansi[pixel];
                    PrintPixel(code);
                }
            }
        }
        private static void PrintPixel(int color)
        {
            Console.Write($"\x1b[38;5;{color}m█\x1b[0m");
        }
    }
}