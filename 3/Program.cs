using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Problem3
{
    class Program
    {
        private static List<string> mapRows;
        private const char Tree = '#';

        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Provide input file name.");
                return;
            }

            mapRows = new List<string>();

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    mapRows.Add(fileStream.ReadLine());
                }
            }

            var slopes = new List<Slope>
            {
                new Slope(1, 1),
                new Slope(3, 1),
                new Slope(5, 1),
                new Slope(7, 1),
                new Slope(1, 2)
            };

            var results = new List<long>();
            foreach (var slope in slopes)
            {
                results.Add(CountTreesForSlope(slope));
            }

            Console.WriteLine(results.Aggregate((a, b) => a * b));
        }

        private static int CountTreesForSlope(Slope slope)
        {
            var currentRow = 0;
            var currentColumn = 0;
            var treeHitCounter = 0;
            var totalProvidedColumns = mapRows.First().Length;

            while (currentRow < mapRows.Count - slope.Y)
            {
                currentColumn += slope.X;
                currentRow += slope.Y;

                if (mapRows[currentRow][currentColumn % totalProvidedColumns] == Tree)
                {                    
                    treeHitCounter++;
                }
            }

            Console.WriteLine("{0} hits for slope {1}", treeHitCounter, slope);

            return treeHitCounter;
        }
    }

    class Slope
    {
        public Slope(int x, int y)
        {
            X = x;
            Y = y;
        } 

        public int X { get; }
        public int Y { get; }

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1}", X, Y);
        }
    }
}
