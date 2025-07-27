namespace sharp_render.src.Common
{
    public sealed class LoggingSingleton
    {
        private static readonly Lazy<LoggingSingleton> lazy = new(() => new LoggingSingleton());

        public static LoggingSingleton Instance
        {
            get { return lazy.Value; }
        }

        private LoggingSingleton()
        {
            Level = LoggingLevels.Info;
        }

        public LoggingLevels Level;
    }
}
