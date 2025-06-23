using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public class CoerceColors
    {
        private readonly Color[,] colorInput;
        private readonly Color[] colorsValid;
        public readonly Color[,] Result;

        public CoerceColors(Color[,] inputColors, Color[] validColors)
        {
            colorInput = inputColors;
            colorsValid = validColors;
            Result = new Color[inputColors.GetLength(0), inputColors.GetLength(1)];
            FillResult();
        }
        private void FillResult()
        {
            foreach (int x in Enumerable.Range(0, colorInput.GetLength(0)))
            {
                foreach (int y in Enumerable.Range(0, colorInput.GetLength(1)))
                {
                    Color input = colorInput[x, y];
                    Result[x, y] = FindNearest(input);
                }
            }
        }
        private Color FindNearest(Color input)
        {
            Dictionary<Color, int> Differences = [];
            foreach (Color termColor in colorsValid)
            {
                Differences[termColor] =
                  Math.Abs(input.R - termColor.R) +
                  Math.Abs(input.B - termColor.B) +
                  Math.Abs(input.G - termColor.G);
            }
            return Differences.MinBy(kvp => kvp.Value).Key;
        }
    }
}