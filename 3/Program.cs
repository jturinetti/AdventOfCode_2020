using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Problem3
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Provide input file name.");
                return;
            }

            var mapRows = new List<string>();

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    mapRows.Add(fileStream.ReadLine());
                }
            }

            var currentRow = 0;
            var currentColumn = 0;
            var treeHitCounter = 0;
            var totalProvidedColumns = mapRows.First().Length;

            const int SlopeX = 3;
            const int SlopeY = 1;
            const char Tree = '#';

            while (currentRow < mapRows.Count - SlopeY)
            {
                currentColumn += SlopeX;
                currentRow += SlopeY;

                if (mapRows[currentRow][currentColumn % totalProvidedColumns] == Tree)
                {                    
                    treeHitCounter++;
                }
            }

            Console.WriteLine(treeHitCounter);
        }
    }
}
