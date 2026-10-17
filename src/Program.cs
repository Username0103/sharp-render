using sharp_render.src.Common;

namespace sharp_render.src
{
    class Program
    {
        private static readonly string help =
            "sharp-render [-h | --help] [-d | --dither] [--profile] PathToBMPFile";

        public static void Main(string[] args)
        {
            var timer = new ProgramTimer();
            string path = ParseArgs(args, out bool shouldDither);
            CheckIfIsBmp(path);
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

        private static void CheckIfIsBmp(string path)
        {
            if (!path.EndsWith(".bmp"))
            {
                Console.Error.WriteLine($"File must be a \".bmp\" file.");
                Environment.Exit(1);
            }
        }

        private static string ParseArgs(string[] args, out bool shouldDither)
        {
            shouldDither = false;
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var isRecognized = false;
                if ((arg == "-h") || (arg == "--help"))
                {
                    Console.WriteLine(help);
                    return "";
                }
                if ((arg == "-d") || (arg == "--dither"))
                {
                    shouldDither = true;
                    isRecognized = true;
                }
                if ((arg == "--profile"))
                {
                    LoggingSingleton.Instance.Level = LoggingLevels.Debug;
                    isRecognized = true;
                }
                if (i == args.Length - 1 && File.Exists(arg))
                {
                    return arg;
                }
                else if (i == args.Length - 1 && !arg.StartsWith("-") && !File.Exists(arg))
                {
                    Console.Error.WriteLine("BMP not found.");
                    return "";
                }
                if (!arg.StartsWith("-") && File.Exists(arg))
                {
                    Console.Error.WriteLine("BMP path must be at the end.");
                    return "";
                }
                if (!isRecognized)
                {
                    Console.Error.WriteLine($"Invalid argument: {arg}");
                    return "";
                }
            }
            Console.WriteLine(help);
            Environment.Exit(0);
            return ""; // for compiler
        }
    }
}
