using sharp_render.src.Common;

namespace sharp_render.src
{
    class Program
    {
        private static readonly string help =
            "sharp-render [-h | --help] [-d | --dither] [-p | [--profile] [REQUIRED <PathToBMPFile>]";

        public static void Main(string[] args)
        {
            var timer = new ProgramTimer();
            string path = ParseArgs(args, out bool shouldDither);
            timer.Start("Total runtime");
            if (path.Length == 0)
            {
                return;
            }
            var image = IMGParse.Orchestrator.Orchestrate(path, out var timeTakenImage);
            var validColors = TermColors.GetColors();
            var printable = IMGHandle.Orchestrator.HandleImage(
                Image: image,
                validColors: [.. validColors.Keys],
                shouldDither: shouldDither,
                timeTaken: out var timeTakenHandling
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

        private static string ParseArgs(string[] args, out bool shouldDither)
        {
            int i = 0;
            shouldDither = false;
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
                if ((arg == "-d") || (arg == "--dither"))
                {
                    shouldDither = true;
                }
                else if ((arg == "-p") || (arg == "--profile"))
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
