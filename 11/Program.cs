using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AocSolution
{
    class Program
    {
        const char FLOOR = '.';
        const char EMPTY_SEAT = 'L';
        const char OCCUPIED_SEAT = '#';

        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Provide input file name.");
                return;
            }

            var inputData = File.ReadAllLines(args[0]);
            var rowLength = inputData[0].Length;

            var sb = new StringBuilder();
            foreach (var s in inputData)
            {
                sb.Append(s);
            }

            Console.WriteLine("Row Length: {0}", rowLength);
            var seatConfiguration = sb.ToString().ToCharArray();
            Console.WriteLine("String Length: {0}", seatConfiguration.Length);
            
            Console.WriteLine("Starting seat map:");
            PrintSeatConfiguration(seatConfiguration, rowLength);

            Console.WriteLine(DetermineOccupiedSeats(seatConfiguration, rowLength, 1));
        }

        private static int DetermineOccupiedSeats(char[] seatConfiguration, int rowLength, int iteration)
        {
            Console.WriteLine();
            Console.WriteLine($"Starting iteration {iteration}.");
            Console.WriteLine();

            var updatedSeatConfiguration = new char[seatConfiguration.Length];
             
            for (int index = 0; index < seatConfiguration.Length; index++)
            {
                char toAppend = FLOOR;
                if (seatConfiguration[index] == EMPTY_SEAT)
                {
                    toAppend = IsEmptySeatNowOccupied(seatConfiguration, index, rowLength);
                }
                else if (seatConfiguration[index] == OCCUPIED_SEAT)
                {
                    toAppend = IsOccupiedSeatNowEmpty(seatConfiguration, index, rowLength);
                }

                updatedSeatConfiguration[index] = toAppend;
            }
            
            PrintSeatConfiguration(updatedSeatConfiguration, rowLength);

            if (Enumerable.SequenceEqual(seatConfiguration, updatedSeatConfiguration))
            {
                Console.WriteLine("Sequences are identical!");

                // done!
                return updatedSeatConfiguration.Count(c => c == OCCUPIED_SEAT);
            }

            return DetermineOccupiedSeats(updatedSeatConfiguration, rowLength, ++iteration);
        }

        private static char IsEmptySeatNowOccupied(char[] seatConfiguration, int index, int rowLength)
        {
            var occupiedSeatFound = false;
            var topIndex = index - rowLength;
            var bottomIndex = index + rowLength;
            var isLeftColumn = index % rowLength == 0;
            var isRightColumn = (index + 1) % rowLength == 0;

            occupiedSeatFound = 
                // left
                (IsValidIndex(index - 1, seatConfiguration.Length) && !isLeftColumn ? seatConfiguration[index - 1] == OCCUPIED_SEAT : false)
                // upper left
                || (IsValidIndex(topIndex - 1, seatConfiguration.Length) && !isLeftColumn ? seatConfiguration[topIndex - 1] == OCCUPIED_SEAT : false)
                // bottom left
                || (IsValidIndex(bottomIndex - 1, seatConfiguration.Length) && !isLeftColumn ? seatConfiguration[bottomIndex - 1] == OCCUPIED_SEAT : false)
                // right
                || (IsValidIndex(index + 1, seatConfiguration.Length) && !isRightColumn ? seatConfiguration[index + 1] == OCCUPIED_SEAT : false)
                // upper right
                || (IsValidIndex(topIndex + 1, seatConfiguration.Length) && !isRightColumn ? seatConfiguration[topIndex + 1] == OCCUPIED_SEAT : false)
                // bottom right
                || (IsValidIndex(bottomIndex + 1, seatConfiguration.Length) && !isRightColumn ? seatConfiguration[bottomIndex + 1] == OCCUPIED_SEAT : false)
                // up
                || (IsValidIndex(topIndex, seatConfiguration.Length) ? seatConfiguration[topIndex] == OCCUPIED_SEAT : false)
                // bottom
                || (IsValidIndex(bottomIndex, seatConfiguration.Length) ? seatConfiguration[bottomIndex] == OCCUPIED_SEAT : false);

            return occupiedSeatFound ? EMPTY_SEAT : OCCUPIED_SEAT;
        }

        private static char IsOccupiedSeatNowEmpty(char[] seatConfiguration, int index, int rowLength)
        {
            var topIndex = index - rowLength;
            var bottomIndex = index + rowLength;
            var isLeftColumn = index % rowLength== 0;
            var isRightColumn = (index + 1) % rowLength == 0;

            var indicesToCheck = new List<int>();
            
            // left
            if (IsValidIndex(index - 1, seatConfiguration.Length) && !isLeftColumn) indicesToCheck.Add(index - 1);
            // upper left
            if (IsValidIndex(topIndex - 1, seatConfiguration.Length) && !isLeftColumn) indicesToCheck.Add(topIndex - 1);
            // bottom left
            if (IsValidIndex(bottomIndex - 1, seatConfiguration.Length) && !isLeftColumn) indicesToCheck.Add(bottomIndex - 1);
            // right
            if (IsValidIndex(index + 1, seatConfiguration.Length) && !isRightColumn) indicesToCheck.Add(index + 1);
            // upper right
            if (IsValidIndex(topIndex + 1, seatConfiguration.Length) && !isRightColumn) indicesToCheck.Add(topIndex + 1);
            // bottom right
            if (IsValidIndex(bottomIndex + 1, seatConfiguration.Length) && !isRightColumn) indicesToCheck.Add(bottomIndex + 1);
            // up
            if (IsValidIndex(topIndex, seatConfiguration.Length)) indicesToCheck.Add(topIndex);
            // bottom
            if (IsValidIndex(bottomIndex, seatConfiguration.Length)) indicesToCheck.Add(bottomIndex);
            
            return indicesToCheck.Count(i => seatConfiguration[i] == OCCUPIED_SEAT) >= 4 ? EMPTY_SEAT : OCCUPIED_SEAT;
        }

        private static bool IsValidIndex(int index, int seatConfigLength)
        {
            return index >= 0 && index < seatConfigLength;
        }

        private static void PrintSeatConfiguration(char[] seatConfiguration, int rowLength)
        {
            var index = 0;
            while (index < seatConfiguration.Length)
            {
                Console.WriteLine(new string(seatConfiguration.Skip(index).Take(rowLength).ToArray()));
                index += rowLength;
            }
        }
    }
}
