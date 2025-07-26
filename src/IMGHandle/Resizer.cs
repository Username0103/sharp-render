using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public class Resizer : Timeable
    {
        private readonly int dataNumRows;
        private readonly int dataNumCols;
        public Color[,] Result;

        public Resizer(Color[,] data, int targetRows, int targetColumns)
            : base("Resizing image")
        {
            Color[,] target = new Color[targetRows, targetColumns];
            dataNumRows = data.GetLength(0);
            dataNumCols = data.GetLength(1);
            var targetRatioX = dataNumRows / (float)targetRows;
            var targetRatioY = dataNumCols / (float)targetColumns;

            for (int x = 0; x < targetRows; x++)
            {
                for (int y = 0; y < targetColumns; y++)
                {
                    float sourceX = x * targetRatioX;
                    float sourceY = y * targetRatioY;
                    float distanceX = sourceX - (int)sourceX;
                    float distanceY = sourceY - (int)sourceY;
                    Color[] sources = GetSources((int)sourceX, (int)sourceY, data);
                    float[] weights = GetWeights(distanceX, distanceY);
                    target[x, y] = Interpolate(sources, weights);
                }
            }
            Result = target;
            Finish();
        }

        private static Color Interpolate(Color[] sources, float[] weights)
        {
            float red = 0;
            float blu = 0;
            float gre = 0;
            for (int i = 0; i < 4; i++)
            {
                red += sources[i].R * weights[i];
                blu += sources[i].B * weights[i];
                gre += sources[i].G * weights[i];
            }
            return new Color([(int)red, (int)gre, (int)blu]);
        }

        private static float[] GetWeights(float distanceX, float distanceY)
        {
            return
            [
                (1 - distanceX) * (1 - distanceY),
                (1 - distanceX) * distanceY,
                distanceX * (1 - distanceY),
                distanceX * distanceY,
            ];
        }

        private Color[] GetSources(int sourceX, int sourceY, Color[,] data)
        {
            return
            [
                data[sourceX, sourceY],
                data[Math.Min(sourceX + 1, dataNumRows - 1), sourceY],
                data[sourceX, Math.Min(sourceY + 1, dataNumCols - 1)],
                data[
                    Math.Min(sourceX + 1, dataNumRows - 1),
                    Math.Min(sourceY + 1, dataNumCols - 1)
                ],
            ];
        }
    }
}
