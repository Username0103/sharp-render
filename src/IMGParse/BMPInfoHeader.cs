namespace sharp_render.src.IMGParse
{
    public class InfoHeader
    {
        public int HeaderSize { get; }
        public int Width { get; }
        public int Height { get; }
        public int BitsPerPixel { get; }
        private readonly int CompressionLevel;
        private readonly string[] RawData;

        public InfoHeader(string[] Data)
        {
            RawData = Data;
            HeaderSize = Utils.HxToInt(RawData[..4]);
            Width = Utils.HxToInt(RawData[4..8]);
            Height = Utils.HxToInt(RawData[8..12]);
            BitsPerPixel = Utils.HxToInt(RawData[14..16]);
            CompressionLevel = Utils.HxToInt(RawData[16..20]);
            Verify();
        }

        private void Verify()
        {
            if (HeaderSize != RawData.Length)
            {
                throw new BadImageFormatException(
                    $"InfoHeader size mismatch. Expected {HeaderSize}, got {RawData.Length}"
                );
            }
            if (BitsPerPixel != 24)
            {
                throw new NotSupportedException(
                    "Program does not support non-truecolor bitmap."
                        + " See https://online-converting.com/image/convert2bmp/ for converting it to one."
                );
            }
            if (CompressionLevel > 0)
            {
                throw new NotSupportedException(
                    "Program does not support compressed bitmap."
                        + " See https://online-converting.com/image/convert2bmp/ for removing compression."
                );
            }
        }
    }
}
