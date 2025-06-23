using sharp_render.src.Common;
using sharp_render.src.IMGHandle;
using sharp_render.src.IMGParse;

namespace sharp_render.src
{
    public class Test
    {
        public Test()
        {
            BMPFIle TestFile = new("""../../../test/Sample2.bmp""");
            Console.WriteLine("Results:");
            Console.WriteLine($"Signature: {string.Join(", ", TestFile.Header.Signature)}");
            Console.WriteLine($"OffsetBytes: {TestFile.Header.OffsetBytes}");
            Console.WriteLine($"HeaderSize: {TestFile.Info.HeaderSize}");
            Console.WriteLine($"Width: {TestFile.Info.Width}");
            Console.WriteLine($"Height: {TestFile.Info.Height}");
            Console.WriteLine($"BitsPerPixel: {TestFile.Info.BitsPerPixel}");
            Console.WriteLine($"Bytes in image: {TestFile.IMGHx.Length}");

            ImgReader BMPRead = new(TestFile);
            Color[,] matrix = new Resizer(BMPRead.Result, 50, 30).Result;
            //Color[,] matrix = BMPRead.Result;

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            using StreamWriter writer = new("./debug.txt");
            writer.WriteLine("[");
            for (int i = 0; i < rows; i++)
            {
                writer.Write("  [ ");
                for (int j = 0; j < cols; j++)
                {
                    writer.Write(matrix[i, j].ToString());
                    if (j < cols - 1)
                    {
                        writer.Write(", ");
                    }
                }
                writer.Write(" ]");
                if (i < rows - 1)
                {
                    writer.WriteLine(",");
                }
                else
                {
                    writer.WriteLine();
                }
            }
            writer.WriteLine("]");

        }
    }
}