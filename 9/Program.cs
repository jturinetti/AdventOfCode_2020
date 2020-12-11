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
            long offendingNumber = 0;

            for (var i = 0; i < PreambleLength; i++)
            {
                inputQueue.Enqueue(Convert.ToInt64(inputData[i]));
            }

            // part 1
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
                    offendingNumber = nextNumber;
                    Console.WriteLine("Number Found! {0}", nextNumber);
                    break;
                }

                inputQueue.Enqueue(nextNumber);
                inputQueue.Dequeue();
                currentIndex++;
            }

            // part 2
            currentIndex = 0;
            var finalIndex = 0;
            long smallestNumber = 0;
            long largestNumber = 0;
            var indicesFound = false;
            while (!indicesFound && currentIndex < inputData.Length)
            {
                finalIndex = currentIndex;
                long sum = 0;
                smallestNumber = 0;
                while (finalIndex < inputData.Length && sum < offendingNumber)
                {
                    var currentNumber = Convert.ToInt64(inputData[finalIndex]);

                    if (currentNumber > largestNumber)
                    {
                        largestNumber = currentNumber;
                    }

                    if (smallestNumber == 0 || currentNumber < smallestNumber)
                    {
                        smallestNumber = currentNumber;
                    }

                    sum += currentNumber;
                    if (sum == offendingNumber)
                    {
                        Console.WriteLine("Found Sum!");
                        Console.WriteLine("Starting Index: {0}", currentIndex);
                        Console.WriteLine("Ending Index: {0}", finalIndex);
                        Console.WriteLine("Smallest Number in Range: {0}", smallestNumber);
                        Console.WriteLine("Largest Number in Range: {0}", largestNumber);
                        indicesFound = true;                        
                    }

                    if (!indicesFound)
                    {
                        finalIndex++;
                    }                    
                }
                if (!indicesFound)
                {
                    currentIndex++;
                }                
            }
            
            Console.WriteLine(smallestNumber + largestNumber);
        }
    }
}
