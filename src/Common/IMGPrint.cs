using System.Text;

namespace sharp_render.src.Common
{
    public class IMGPrint
    {
        public static void Print(Color[,] image, Dictionary<Color, int> color2Ansi)
        {
            StringBuilder builder = new();
            foreach (int x in Enumerable.Range(0, image.GetLength(0)))
            {
                builder.Append('\n');
                foreach (int y in Enumerable.Range(0, image.GetLength(1)))
                {
                    Color pixel = image[x, y];
                    int code = color2Ansi[pixel];
                    builder.Append(CodeToPrint(code));
                }
            }
            //Console.WriteLine(builder.ToString());
        }
        private static string CodeToPrint(int color)
        {
            return $"\x1b[38;5;{color}m█\x1b[0m";
        }
    }
}