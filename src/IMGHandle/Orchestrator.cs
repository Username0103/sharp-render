using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public static class Orchestrator
    {
        public static Color[,] HandleImage(Color[,] Image, Color[] validColors, out long timeTaken)
        {
            var dimensions = GetTermDimensions();
            var resized = Resizer.Resize(
                Image,
                dimensions[0],
                dimensions[1],
                out var timeTakenResizing
            );
            var reduced = ReduceColors.Reduce(resized, validColors, out var timeTakenReducing);
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
