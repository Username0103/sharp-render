using System.Text;

namespace sharp_render.src.Common
{
    public static class IMGPrint
    {
        public static void Print(
            Color[,] image,
            Dictionary<Color, int> color2Ansi,
            out long timeTaken
        )
        {
            var timer = new ProgramTimer();
            timer.Start("Turning finished image into text");
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
            timeTaken = timer.Finish();
            if (LoggingSingleton.Instance.Level <= LoggingLevels.Info)
            {
                Console.WriteLine(builder.ToString());
            }
        }

        private static string CodeToPrint(int color)
        {
            return $"\x1b[38;5;{color}m█\x1b[0m";
        }
    }
}
