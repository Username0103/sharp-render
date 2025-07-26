using System.Diagnostics;

namespace sharp_render.src.Common
{
    public abstract class Timeable
    {
        protected string Name;
        private readonly Stopwatch timer;

        protected Timeable(string operationName)
        {
            Name = operationName;
            timer = new();
            if (LoggingSingleton.Instance.Level == LoggingLevels.Debug)
            {
                timer.Start();
            }
        }

        protected void Finish()
        {
            if (LoggingSingleton.Instance.Level >= LoggingLevels.Debug)
            {
                timer.Stop();
                Console.WriteLine($"{Name}: elapsed {timer.ElapsedMilliseconds}ms");
            }
        }
    }

    public abstract class TimeableNoConstructor
    {
        protected string? Name;
        private Stopwatch? timer;

        protected void Start(string operationName)
        {
            Name = operationName;
            timer = new();
            if (LoggingSingleton.Instance.Level == LoggingLevels.Debug)
            {
                timer.Start();
            }
        }

        protected void Finish()
        {
            if (LoggingSingleton.Instance.Level >= LoggingLevels.Debug)
            {
                if (Name != null && timer != null)
                {
                    timer.Stop();
                    Console.WriteLine($"{Name}: elapsed {timer.ElapsedMilliseconds}ms");
                }
            }
        }
    }
}
