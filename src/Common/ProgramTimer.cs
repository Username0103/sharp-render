using System.Diagnostics;

namespace sharp_render.src.Common
{
    public sealed class ProgramTimer
    {
        private string Name = "";
        private readonly Stopwatch timer = new();

        public void Start(string operationName)
        {
            Name = operationName;
            if (LoggingSingleton.Instance.Level >= LoggingLevels.Debug)
            {
                timer.Start();
            }
        }

        public void Start()
        {
            if (LoggingSingleton.Instance.Level >= LoggingLevels.Debug)
            {
                timer.Start();
            }
        }

        public void Stop()
        {
            if (LoggingSingleton.Instance.Level >= LoggingLevels.Debug)
            {
                timer.Stop();
            }
        }

        public long Finish()
        {
            if (LoggingSingleton.Instance.Level >= LoggingLevels.Debug)
            {
                timer.Stop();
                Console.WriteLine($"{Name}: elapsed {timer.ElapsedMilliseconds}ms");
            }
            return timer.ElapsedMilliseconds;
        }
    }
}
