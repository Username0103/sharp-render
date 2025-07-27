using sharp_render.src.Common;

namespace sharp_render.src
{
    class Program : TimeableNoConstructor
    {
        private static readonly string help =
            """sharp-render [-h || --help] [--debug || -d] [REQUIRED <PathToBMPFile>]""";

        public static void Main(string[] args)
        {
            Program self = new();
            string path = ParseArgs(args);
            self.Start("Total runtime");
            if (path.Length == 0)
            {
                return;
            }
            IMGParse.Orchestrator Image = new(path);
            IMGHandle.Orchestrator Printable = new(Image.Result);
            IMGPrint.Print(Printable.Result, Printable.TermColorsResult);
            self.Finish();
        }

        private static string ParseArgs(string[] args)
        {
            string path = "";
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
                else
                {
                    return arg;
                }
                i++;
            }
            return path;
        }
    }
}
