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
            var seatIdList = new List<int>();

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    var boardingPassEntry = ConvertToBinary(fileStream.ReadLine());
                    
                    var row = boardingPassEntry.Substring(0, 7);
                    var column = boardingPassEntry.Substring(7);

                    var rowByte = Convert.ToByte(row, 2);                    
                    var columnByte = Convert.ToByte(column, 2);
                    
                    var seatId = (rowByte * 8) + columnByte;
                    seatIdList.Add(seatId);
                }

                seatIdList.Sort();
                var index = 1;

                while (index < seatIdList.Count - 2)
                {
                    var currentSeatId = seatIdList[index];
                    if (currentSeatId - seatIdList[index - 1] != 1)
                    {
                        Console.WriteLine("My Seat ID: {0}", currentSeatId - 1);
                        break;
                    }                    

                    index++;
                }
                
                Console.WriteLine("Maximum Seat ID: {0}", seatIdList.Last());
                                
            }
        }

        private static string ConvertToBinary(string boardingPassEntry)
        {
            return boardingPassEntry.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1');
        }
    }
}
