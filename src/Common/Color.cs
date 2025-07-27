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

        public Color(float r, float g, float b)
        {
            R = (byte)(r * 255);
            G = (byte)(g * 255);
            B = (byte)(b * 255);
        }

        public Color(float[] colors)
        {
            R = (byte)(colors[0] * 255);
            G = (byte)(colors[1] * 255);
            B = (byte)(colors[2] * 255);
        }

        public Color(byte[] colors)
        {
            R = colors[0];
            G = colors[1];
            B = colors[2];
        }
    }
}
