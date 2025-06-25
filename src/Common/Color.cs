namespace sharp_render.src.Common
{
    public record Color
    {
        public int R;
        public int G;
        public int B;

        public Color(int[] colors)
        {
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