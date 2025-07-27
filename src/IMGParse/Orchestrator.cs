using sharp_render.src.Common;

namespace sharp_render.src.IMGParse
{
    public static class Orchestrator
    {
        public static Color[,] Orchestrate(string path, out long timeTaken)
        {
            var timer = new ProgramTimer();
            timer.Start("Reading image");
            var file = new BMPFIle(path);
            var oneDimensionalImage = ImgReader.Extract1D(file);

            var twoDimensionalImage = ImgReader.To2D(
                oneDimensionalImage,
                file.Info.Height,
                file.Info.Width
            );
            timeTaken = timer.Finish();
            return twoDimensionalImage;
        }
    }
}
