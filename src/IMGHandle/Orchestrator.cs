using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public static class Orchestrator
    {
        public static Color[,] HandleImage(
            Color[,] Image,
            Color[] validColors,
            bool shouldDither,
            out long timeTaken
        )
        {
            var dimensions = GetTermDimensions();
            var resized = Resizer.Resize(
                data: Image,
                targetRows: dimensions[0] * 2,
                targetColumns: dimensions[1],
                timeTaken: out var timeTakenResizing
            );

            Color[,] reduced;
            long timeTakenReducing;

            if (shouldDither)
            {
                reduced = ReduceColorsAndDither.Reduce(resized, validColors, out timeTakenReducing);
            }
            else
            {
                reduced = ReduceColors.Reduce(resized, validColors, out timeTakenReducing);
            }

            timeTaken = timeTakenResizing + timeTakenReducing;
            return reduced;
        }

        private static int[] GetTermDimensions()
        {
            if (LoggingSingleton.Instance.Level == LoggingLevels.Debug)
            {
                return [500, 500]; // image will not get printed at the end.
            }
            return [Console.WindowHeight, Console.WindowWidth];
        }
    }
}
