namespace sharp_render.src.IMGParse
{
    public record class BMPFile
    {
        public string[] HxArr { get; }
        public BMPHeader Header { get; }
        public InfoHeader Info { get; }
        public string[] IMGHx { get; }

        public BMPFile(string BMPPath)
        {
            HxArr = Utils.ReadImg(BMPPath);
            Header = new(HxArr[..14]);
            Info = new(HxArr[14..Header.OffsetBytes]);
            IMGHx = HxArr[Header.OffsetBytes..];
        }
    }
}
