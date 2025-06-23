

namespace sharp_render.src
{
    class Program
    {
        private readonly string help = """sharp-render ([(-h || --help)] [(-p || --path) <PathToBMPFile>]) || [<PathToBMPFile>]""";
        static void Main(string[] args)
        {
            Program self = new();
            string path = self.ParseArgs(args);
            if (path.Length == 0) { return; }
            IMGParse.Orchestrator Image = new(path);
            IMGHandle.Orchestrator Printable = new(Image.Result);
            Common.IMGPrint.Print(Printable.Result, Printable.TermColorsResult);
        }
        private string ParseArgs(string[] args)
        {
            bool isPath = false;
            string path = "";
            int i = 0;
            if (args.Length == 0) { Console.WriteLine(help); return ""; }
            foreach (string arg in args)
            {
                if ((arg == "-h") || (arg == "--help")) { Console.WriteLine(help); }
                else if ((arg == "-p") || (arg == "--path")) { isPath = true; }
                else if (isPath) { path = arg; isPath = false; }
                else if (i == 0) { return arg; }
                i++;
            }
            return path;
        }
    }
}