using System.Collections.Concurrent;
using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public static class ReduceColors
    {
        private static readonly ConcurrentDictionary<Color, Color> colorCache = [];

        public static Color[,] Reduce(Color[,] inputColors, Color[] validColors, out long timeTaken)
        {
            var timer = new ProgramTimer();
            timer.Start("Color reduction");
            var result = new Color[inputColors.GetLength(0), inputColors.GetLength(1)];
            FillResult(ref result, inputColors, validColors);
            timeTaken = timer.Finish();
            return result;
        }

        private static void FillResult(ref Color[,] result, Color[,] colorInput, Color[] valid)
        {
            var colorTasks = new Task<FindNearestOutput>[
                colorInput.GetLength(0) * colorInput.GetLength(1)
            ];
            int i = 0;

            foreach (int x in Enumerable.Range(0, colorInput.GetLength(0)))
            {
                foreach (int y in Enumerable.Range(0, colorInput.GetLength(1)))
                {
                    Color input = colorInput[x, y];
                    colorTasks[i] = Task.Run(() => FindNearest(input, valid, x, y));
                    i++;
                }
            }
            var colorResults = Task.WhenAll(colorTasks);
            colorResults.Wait();
            FindNearestOutput[] awaitedResult = colorResults.Result;
            foreach (var output in awaitedResult)
            {
                result[output.x, output.y] = output.result;
            }
        }

        private static FindNearestOutput FindNearest(Color input, Color[] colorsValid, int x, int y)
        {
            if (colorCache.TryGetValue(input, out Color cacheResult))
            {
                return new(cacheResult, x, y);
            }

            Dictionary<Color, double> Differences = [];
            foreach (var termColor in colorsValid)
            {
                Differences[termColor] = CieDe2000Comparison.CalculateDeltaE(input, termColor);
            }
            Color output = Differences.MinBy(kvp => kvp.Value).Key;
            colorCache.TryAdd(input, output);
            return new(output, x, y);
        }

        private readonly struct FindNearestOutput(Color result, int x, int y)
        {
            public readonly Color result = result;
            public readonly int x = x,
                y = y;
        }
    }
}
