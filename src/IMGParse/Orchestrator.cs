using sharp_render.src.Common;

namespace sharp_render.src.IMGParse
{
    public class Orchestrator
    {

        public Color[,] Result { get; }

        public Orchestrator(string path)
        {
            BMPFIle file = new(path);
            ImgReader BMPRead = new(file);
            Result = BMPRead.Result;
        }
    }
}