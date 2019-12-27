using System;
using System.Linq;
using System.Text;

namespace AristotleHoneyComb
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = Enumerable.Range(1, 19).ToArray();

            HoneyComb honeyComb = new HoneyComb();

            Console.WriteLine(honeyComb.Print());

            Console.WriteLine($"Done @ {DateTime.Now}");
            Console.ReadLine();
        }
    }

    class HoneyComb
    {
        public int[][] Rows { get; } = new int[5][];

        public HoneyComb()
        {
            Rows[0] = new int[3];
            Rows[1] = new int[4];
            Rows[2] = new int[5];
            Rows[3] = new int[4];
            Rows[4] = new int[3];
        }

        public string Print()
        {
            StringBuilder printer = new StringBuilder();

            int cellLength = 4;
            int maxRowLength = 5 * cellLength;

            printer.AppendLine().AppendLine();

            for (int rowIndex = 0; rowIndex < Rows.Length; rowIndex++)
            {
                int padding = (maxRowLength - Rows[rowIndex].Length * cellLength) / 2;

                printer.Append(new string(' ', padding));

                for (int i = 0; i < Rows[rowIndex].Length; i++)
                {
                    printer.Append(Rows[rowIndex][i].ToString(" 00 "));
                }
                printer.AppendLine().AppendLine();
            }

            return printer.ToString();
        }
    }
}
