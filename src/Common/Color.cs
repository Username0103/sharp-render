namespace sharp_render.src.Common
{
    public record Color
    {
        public int R;
        public int G;
        public int B;

        public Color(int[] colors)
        {
            for (int i = 0; i < colors.Length - 1; i++)
            {
                if (colors[i] > 255 || colors[i] < 0)
                {
                    throw new OverflowException($"Tried to set a color to {colors[i]}."
                    + $"\nRGB = {colors[0]}-{colors[1]}-{colors[2]}");
                }
            }
            R = colors[0];
            G = colors[1];
            B = colors[2];
        }
        public override string ToString()
        {
            return $"{R} {G} {B}";
        }
    }
}