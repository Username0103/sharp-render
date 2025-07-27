namespace sharp_render.src.Common
{
    public struct Color
    {
        public byte R;
        public byte G;
        public byte B;

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Color(byte[] colors)
        {
            R = colors[0];
            G = colors[1];
            B = colors[2];
        }

        public Color(int r, int g, int b)
        {
            R = (byte)r;
            G = (byte)g;
            B = (byte)b;
        }
    }
}
