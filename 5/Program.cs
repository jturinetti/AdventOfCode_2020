using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Problem5
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

            var maximumSeatId = 0;

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    var boardingPassEntry = ConvertToBinary(fileStream.ReadLine());
                    
                    var row = boardingPassEntry.Substring(0, 7);
                    var column = boardingPassEntry.Substring(7);

                    var rowByte = Convert.ToByte(row, 2);
                    Console.WriteLine(rowByte);

                    var columnByte = Convert.ToByte(column, 2);
                    Console.WriteLine(columnByte);
                    var seatId = (rowByte * 8) + columnByte;
                    Console.WriteLine("Seat ID: {0}", seatId);

                    if (seatId > maximumSeatId)
                    {
                        maximumSeatId = seatId;
                    }
                }

                Console.WriteLine("Maximum Seat ID: {0}", maximumSeatId);
            }
        }

        private static string ConvertToBinary(string boardingPassEntry)
        {
            return boardingPassEntry.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1');
        }
    }
}
