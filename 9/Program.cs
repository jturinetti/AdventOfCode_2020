using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AocSolution
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

            var inputData = File.ReadAllLines(args[0]);

            const int PreambleLength = 25;
            var inputQueue = new Queue<long>();

            var currentIndex = PreambleLength;

            for (var i = 0; i < PreambleLength; i++)
            {
                inputQueue.Enqueue(Convert.ToInt64(inputData[i]));
            }

            while (currentIndex < inputData.Length)
            {
                var nextNumber = Convert.ToInt64(inputData[currentIndex]);
                var sortedDataList = inputQueue.ToList();
                sortedDataList.Sort();

                var nextNumberAddendsFound = false;
                var addendIndex1 = 0;
                var addendIndex2 = 1;

                while (!nextNumberAddendsFound 
                        && addendIndex1 < sortedDataList.Count
                        && sortedDataList[addendIndex1] <= nextNumber)
                {
                    nextNumberAddendsFound = sortedDataList[addendIndex1] + sortedDataList[addendIndex2] == nextNumber;

                    if (!nextNumberAddendsFound)
                    {
                        if (addendIndex2 == sortedDataList.Count - 1 || sortedDataList[addendIndex2] > nextNumber)
                        {
                            addendIndex1++;
                            addendIndex2 = addendIndex1 + 1;
                        }
                        else
                        {
                            addendIndex2++;
                        }
                    }
                }

                if (!nextNumberAddendsFound)
                {
                    Console.WriteLine("Number Found! {0}", nextNumber);
                    break;
                }

                inputQueue.Enqueue(nextNumber);
                inputQueue.Dequeue();
                currentIndex++;
            }            
        }
    }
}
