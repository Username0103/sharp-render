using System.Globalization;

namespace sharp_render.src.IMGParse
{
    public class Utils
    {
        public static int HxToInt(string[] hexNumber)
        {
            IEnumerable<string> reversed = hexNumber.Reverse();
            string joined = string.Join("", reversed);
            return int.Parse(joined, NumberStyles.HexNumber);
        }

        public static string[] ReadImg(string BMPPath)
        {
            byte[] bytes = File.ReadAllBytes(BMPPath);
            string hexData = BitConverter.ToString(bytes);
            return hexData.Split('-');
        }

        public static T[,] ReverseMatrix<T>(T[,] matrix)
        {
            T[,] result = new T[matrix.GetLength(0), matrix.GetLength(1)];
            int j = 0;
            for (int i = matrix.GetLength(0) - 1; i >= 0; i--)
            {
                IEnumerable<T> row = Enumerable
                    .Range(0, matrix.GetLength(1))
                    .Select(x => matrix[i, x]);
                int k = 0;
                foreach (T elm in row)
                {
                    result[j, k] = elm;
                    k++;
                }
                j++;
            }
            return result;
        }
    }
}
