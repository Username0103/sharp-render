using System.Collections.Concurrent;
using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public class ReduceColors : Timeable
    {
        private readonly Color[,] colorInput;
        private readonly Color[] colorsValid;
        public readonly Color[,] Result;
        private readonly ConcurrentDictionary<Color, Color> colorCache = [];

        public ReduceColors(Color[,] inputColors, Color[] validColors)
            : base("Color reduction")
        {
            colorInput = inputColors;
            colorsValid = validColors;
            Result = new Color[inputColors.GetLength(0), inputColors.GetLength(1)];
            FillResult();
            Finish();
        }

        private void FillResult()
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
                    colorTasks[i] = Task.Run(() => FindNearest(input, x, y));
                    i++;
                }
            }
            var colorResults = Task.WhenAll(colorTasks);
            colorResults.Wait();
            FindNearestOutput[] awaitedResult = colorResults.Result;
            foreach (var output in awaitedResult)
            {
                Result[output.x, output.y] = output.result;
            }
        }

        private FindNearestOutput FindNearest(Color input, int x, int y)
        {
            if (colorCache.TryGetValue(input, out Color? cacheResult))
            {
                return new(cacheResult, x, y);
            }

            Dictionary<Color, double> Differences = [];
            foreach (Color termColor in colorsValid)
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
