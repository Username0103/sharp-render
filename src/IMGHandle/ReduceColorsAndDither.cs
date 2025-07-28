using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public static class ReduceColorsAndDither
    {
        private static readonly Dictionary<Color, Color> colorCache = [];

        public static Color[,] Reduce(Color[,] inputColors, Color[] validColors, out long timeTaken)
        {
            var timer = new ProgramTimer();
            timer.Start("Color reduction");
            FillResult(inputColors, validColors);
            timeTaken = timer.Finish();
            return inputColors;
        }

        private static void FillResult(Color[,] colorInput, Color[] valid)
        {
            var ditherer = new Ditherer();
            var width = colorInput.GetUpperBound(0) + 1;
            var height = colorInput.GetUpperBound(1) + 1;

            foreach (int y in Enumerable.Range(0, height))
            {
                foreach (int x in Enumerable.Range(0, width))
                {
                    Color input = colorInput[x, y];
                    var quantized = Quantize(input, valid);
                    colorInput[x, y] = quantized;
                    ditherer.Dither(
                        data: colorInput,
                        original: input,
                        transformed: quantized,
                        x: x,
                        y: y,
                        width: width,
                        height: height
                    );
                }
            }
        }

        private static Color Quantize(Color input, Color[] colorsValid)
        {
            if (colorCache.TryGetValue(input, out Color cacheResult))
            {
                return cacheResult;
            }
            Color output = colorsValid.MinBy((c) => CieDe2000Comparison.CalculateDeltaE(input, c));
            colorCache.TryAdd(input, output);
            return output;
        }
    }
}
