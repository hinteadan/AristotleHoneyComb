using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AristotleHoneyComb
{
    class Program
    {
        static void Main(string[] args)
        {
            HoneyCombSolver honeyCombSolver = new HoneyCombSolver().Solve();

            Console.WriteLine(honeyCombSolver);

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

        public HoneyComb Fill(params int[][] values)
        {
            for (int rowIndex = 0; rowIndex < Math.Min(values.Length, Rows.Length); rowIndex++)
            {
                for (int i = 0; i < Math.Min(values[rowIndex].Length, Rows[rowIndex].Length); i++)
                {
                    Rows[rowIndex][i] = values[rowIndex][i];
                }
            }

            return this;
        }

        public HoneyComb Rotate()
        {

            return new HoneyComb()
                .Fill(
                    new int[] { Rows[2][0], Rows[1][0], Rows[0][0] }
                    , new int[] { Rows[3][0], Rows[2][1], Rows[1][1], Rows[0][1] }
                    , new int[] { Rows[4][0], Rows[3][1], Rows[2][2], Rows[1][2], Rows[0][2] }
                    , new int[] { Rows[4][1], Rows[3][2], Rows[2][3], Rows[1][3] }
                    , new int[] { Rows[4][2], Rows[3][3], Rows[2][4] }
                );
        }

        public int[] RowSums() => Rows.Select(x => x.Sum()).ToArray();

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

    class HoneyCombSolver
    {
        static readonly int[] numberPool = Enumerable.Range(1, 19).ToArray();

        private readonly HoneyComb honeyComb;

        public HoneyCombSolver()
        {
            honeyComb = new HoneyComb()
                .Fill(
                    Enumerable.Range(1, 3).ToArray(),
                    Enumerable.Range(4, 4).ToArray(),
                    Enumerable.Range(8, 5).ToArray(),
                    Enumerable.Range(13, 4).ToArray(),
                    Enumerable.Range(17, 3).ToArray()
                );
        }

        public override string ToString()
        {
            return honeyComb.Print();
        }

        public HoneyCombSolver Solve()
        {
            int[][] possibleGroupsOfThreeA = CalculatePossibleGroupsOfThree(numberPool);

            foreach (int[] groupOfThreeA in possibleGroupsOfThreeA)
            {
                int[][] possibleGroupsOfThreeB = CalculatePossibleGroupsOfThree(numberPool.Except(groupOfThreeA));

                foreach (int[] groupOfThreeB in possibleGroupsOfThreeB)
                {
                    int[][] possibleGroupsOfFoursA = CalculatePossibleGroupsOfFour(numberPool.Except(groupOfThreeA.Concat(groupOfThreeB)));

                    foreach(int[] groupOfFourA in possibleGroupsOfFoursA)
                    {
                        int[][] possibleGroupsOfFoursB = CalculatePossibleGroupsOfFour(numberPool.Except(groupOfThreeA.Concat(groupOfThreeB).Concat(groupOfFourA)));

                        foreach(int[] groupOfFourB in possibleGroupsOfFoursB)
                        {
                            int[][] possibleGroupsOfFives = CalculatePossibleGroupsOfFive(numberPool.Except(groupOfThreeA.Concat(groupOfThreeB).Concat(groupOfFourA).Concat(groupOfFourB)));

                            foreach(int[] groupOfFive in possibleGroupsOfFives)
                            {
                                Console.WriteLine($"{possibleGroupsOfThreeA.Length} x {possibleGroupsOfThreeB.Length} x {possibleGroupsOfFoursA.Length} x {possibleGroupsOfFoursB.Length} x {possibleGroupsOfFives.Length}");

                                honeyComb.Fill(
                                    groupOfThreeA,
                                    groupOfFourA,
                                    groupOfFive,
                                    groupOfFourB,
                                    groupOfThreeB
                                );

                                if (IsSolved())
                                    return this;
                            }
                        }
                    }

                }
            }

            return this;
        }

        private bool IsSolved()
        {
            return
                honeyComb.RowSums().All(x => x == 38)
                && honeyComb.Rotate().RowSums().All(x => x == 38)
                && honeyComb.Rotate().Rotate().RowSums().All(x => x == 38)
                ;
        }

        private int[][] CalculatePossibleGroupsOfThree(IEnumerable<int> numberPool)
        {
            List<int[]> result = new List<int[]>();

            foreach (int a in numberPool)
            {
                foreach (int b in numberPool)
                {
                    if (b == a) continue;

                    foreach (int c in numberPool)
                    {
                        if (c == a || c == b) continue;
                        if (a + b + c != 38) continue;

                        result.Add(new int[] { a, b, c });
                    }
                }
            }

            return result.ToArray();
        }

        private int[][] CalculatePossibleGroupsOfFour(IEnumerable<int> numberPool)
        {
            List<int[]> result = new List<int[]>();

            foreach (int a in numberPool)
            {
                foreach (int b in numberPool)
                {
                    if (b == a) continue;

                    foreach (int c in numberPool)
                    {
                        if (c == a || c == b) continue;

                        foreach (int d in numberPool)
                        {
                            if (d == a || d == b || d == c) continue;

                            if (a + b + c + d != 38) continue;

                            result.Add(new int[] { a, b, c, d });

                        }

                    }
                }
            }

            return result.ToArray();
        }

        private int[][] CalculatePossibleGroupsOfFive(IEnumerable<int> numberPool)
        {
            List<int[]> result = new List<int[]>();

            foreach (int a in numberPool)
            {
                foreach (int b in numberPool)
                {
                    if (b == a) continue;

                    foreach (int c in numberPool)
                    {
                        if (c == a || c == b) continue;

                        foreach (int d in numberPool)
                        {
                            if (d == a || d == b || d == c) continue;

                            foreach (int e in numberPool)
                            {
                                if (e == a || e == b || e == c || e == d) continue;

                                if (a + b + c + d + e != 38) continue;

                                result.Add(new int[] { a, b, c, d, e });
                            }
                        }

                    }
                }
            }

            return result.ToArray();
        }
    }
}
