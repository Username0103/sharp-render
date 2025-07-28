using sharp_render.src.Common;

namespace sharp_render.src
{
    class Program
    {
        private static readonly string help =
            "sharp-render [-h | --help] [--debug | -d] [REQUIRED <PathToBMPFile>]";

        public static void Main(string[] args)
        {
            var timer = new ProgramTimer();
            string path = ParseArgs(args);
            timer.Start("Total runtime");
            if (path.Length == 0)
            {
                return;
            }
            var image = IMGParse.Orchestrator.Orchestrate(path, out var timeTakenImage);
            var validColors = TermColors.GetColors();
            var printable = IMGHandle.Orchestrator.HandleImage(
                image,
                [.. validColors.Keys],
                out var timeTakenHandling
            );
            IMGPrint.Print(printable, validColors, out var timeTakenPrinter);
            var timeTaken = timer.Finish();
            if (LoggingSingleton.Instance.Level >= LoggingLevels.Debug)
            {
                Console.WriteLine(
                    $"Untimed Sections: elapsed {timeTaken - (timeTakenImage + timeTakenHandling + timeTakenPrinter)}ms"
                );
            }
        }

        private static string ParseArgs(string[] args)
        {
            int i = 0;
            if (args.Length == 0)
            {
                Console.WriteLine(help);
                return "";
            }
            foreach (string arg in args)
            {
                if ((arg == "-h") || (arg == "--help"))
                {
                    Console.WriteLine(help);
                }
                else if ((arg == "-d") || (arg == "--debug"))
                {
                    LoggingSingleton.Instance.Level = LoggingLevels.Debug;
                }
                else if (File.Exists(arg))
                {
                    return arg;
                }
                i++;
            }
            Console.Error.WriteLine("BMP not found.");
            Environment.Exit(0);
            return ""; // for compiler
        }
    }
}
