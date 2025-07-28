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

            int width = image.GetLength(0);
            int height = image.GetLength(1);

            for (int row = 0; row < width; row += 2)
            {
                builder.Append('\n');
                for (int col = 0; col < height; col++)
                {
                    Color top = image[row, col];
                    Color bottom = image[row + 1, col];
                    int topCode = color2Ansi[top];
                    int bottomCode = color2Ansi[bottom];
                    builder.Append(CodeToPrint(topCode, bottomCode));
                }
            }
            timeTaken = timer.Finish();
            if (LoggingSingleton.Instance.Level <= LoggingLevels.Info)
            {
                Console.WriteLine(builder.ToString());
            }
        }

        private static string CodeToPrint(int foreground, int background)
        {
            return $"\x1b[38;5;{background}m\x1b[48;5;{foreground}m▄\x1b[0m\x1b[0m";
        }
    }
}
