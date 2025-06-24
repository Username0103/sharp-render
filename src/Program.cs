
using sharp_render.src.Common;

namespace sharp_render.src
{
    class Program : TimeableNoConstructor
    {
        private readonly string help = """sharp-render [-h || --help] [--debug || -d] [REQUIRED <PathToBMPFile>]""";
        static void Main(string[] args)
        {
            Program self = new();
            string path = self.ParseArgs(args);
            self.Start("Total runtime");
            if (path.Length == 0) { return; }
            IMGParse.Orchestrator Image = new(path);
            IMGHandle.Orchestrator Printable = new(Image.Result);
            IMGPrint.Print(Printable.Result, Printable.TermColorsResult);
            self.Finish();
        }

        private string ParseArgs(string[] args)
        {
            string path = "";
            int i = 0;
            if (args.Length == 0) { Console.WriteLine(help); return ""; }
            foreach (string arg in args)
            {
                if ((arg == "-h") || (arg == "--help")) { Console.WriteLine(help); }
                else if ((arg == "-d") || (arg == "--debug")) { LoggingSingleton.Instance.Level = LoggingLevels.Debug; }
                else { return arg; }
                i++;
            }
            return path;
        }
    }
}