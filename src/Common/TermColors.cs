// https://en.wikipedia.org/wiki/ANSI_escape_code#8-bit

namespace sharp_render.src.Common
{
    public class TermColors
    {
        public Dictionary<Color, int> Result { get; }

        public TermColors()
        {
            Result = [];
            Add4Bit();
            Add8Bit();
            AddGray();
        }

        private void Add4Bit()
        {
            foreach (int code in Enumerable.Range(0, 16))
            {
                int level = code > 8 ? 255 : code == 7 ? 229 : 205;
                int r = code == 8 ? 127 : (code & 1) != 0 ? level : code == 12 ? 92 : 0;
                int g = code == 8 ? 127 : (code & 2) != 0 ? level : code == 12 ? 92 : 0;
                int b = code == 8 ? 127 : code == 4 ? 238 : (code & 4) != 0 ? level : 0;
                Result[new Color([r, g, b])] = code;
            }
        }
        private void Add8Bit()
        {
            int ansiCode = 16;
            foreach (int red in Enumerable.Range(0, 6))
            {
                foreach (int green in Enumerable.Range(0, 6))
                {
                    foreach (int blue in Enumerable.Range(0, 6))
                    {
                        int r = red != 0 ? red * 40 + 55 : 0;
                        int g = green != 0 ? green * 40 + 55 : 0;
                        int b = blue != 0 ? blue * 40 + 55 : 0;
                        Result[new Color([r, g, b])] = ansiCode;
                        ansiCode++;
                    }
                }
            }
        }
        private void AddGray()
        {
            int ansiCode = 232;
            foreach (int neutral in Enumerable.Range(0, 24))
            {
                int level = neutral * 10 + 8;
                Result[new Color([level, level, level])] = ansiCode;
                ansiCode++;
            }
        }
    }
}