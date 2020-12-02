using System;
using System.Collections.Generic;
using System.IO;

namespace Problem1
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

            var inputList = new List<int>();

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    inputList.Add(Convert.ToInt32(fileStream.ReadLine()));
                }                
            }            

            // TwoNumberSolution(inputList);
            ThreeNumberSolution(inputList);
        }

        private static void TwoNumberSolution(List<int> inputList)
        {
            var firstNumberIndex = 0;
            while (firstNumberIndex < inputList.Count - 1)
            {
                for (int secondNumberIndex = firstNumberIndex + 1; secondNumberIndex < inputList.Count; secondNumberIndex++)
                {
                    var num1 = inputList[firstNumberIndex];
                    var num2 = inputList[secondNumberIndex];

                    if (num1 + num2 == 2020)
                    {
                        Console.WriteLine(num1 * num2);
                        return;
                    }
                }
                firstNumberIndex++;
            }       
        }

        private static void ThreeNumberSolution(List<int> inputList)
        {
            var firstNumberIndex = 0;
            while (firstNumberIndex < inputList.Count - 2)
            {
                for (int secondNumberIndex = firstNumberIndex + 1; secondNumberIndex < inputList.Count - 1; secondNumberIndex++)
                {
                    for (int thirdNumberIndex = secondNumberIndex + 1; thirdNumberIndex < inputList.Count; thirdNumberIndex++)
                    {
                        var num1 = inputList[firstNumberIndex];
                        var num2 = inputList[secondNumberIndex];
                        var num3 = inputList[thirdNumberIndex];

                        if (num1 + num2 + num3 == 2020)
                        {
                            Console.WriteLine(num1 * num2 * num3);
                            return;
                        }
                    }
                    secondNumberIndex++;                    
                }
                firstNumberIndex++;
            }       
        }
    }
}
