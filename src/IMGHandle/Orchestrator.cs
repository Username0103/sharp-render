using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public class Orchestrator
    {
        public Color[,] Result;
        public Dictionary<Color, int> TermColorsResult;

        public Orchestrator(Color[,] Image)
        {
            int[] dimensions = GetTermDimensions();
            TermColors colors = new();
            Color[] validColors = [.. colors.Result.Keys];
            TermColorsResult = colors.Result;
            Color[,] resized = new Resizer(Image, dimensions[0], dimensions[1]).Result;
            Color[,] coerced = new CoerceColors(resized, validColors).Result;
            Result = coerced;
        }
        private static int[] GetTermDimensions()
        {
            return [Console.WindowHeight, Console.WindowWidth];
        }
    }
}