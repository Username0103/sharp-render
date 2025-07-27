namespace sharp_render.src.IMGParse
{
    public record class BMPHeader
    {
        public string[] Signature { get; private set; }
        public int OffsetBytes { get; private set; }

        public BMPHeader(string[] Header)
        {
            Signature = Header[0..2];
            VerifyHeader();
            OffsetBytes = Utils.HxToInt(Header[10..14]);
        }

        private void VerifyHeader()
        {
            string[] Expected = ["42", "4D"];
            if (!Signature.SequenceEqual(Expected))
            {
                throw new FileLoadException(
                    $"Expected {string.Join(", ", Expected)}, got {string.Join(", ", Signature)}"
                );
            }
        }
    }
}
