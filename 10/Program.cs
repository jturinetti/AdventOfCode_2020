using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AocSolution
{
    class Program
    {
        static Dictionary<int, long> cache = new Dictionary<int, long>();

        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Provide input file name.");
                return;
            }

            var joltages = File.ReadAllLines(args[0]).Select(j => Convert.ToInt32(j)).ToList();
            joltages.Sort();
            
            var oneJoltDiffs = 1; // first number is 1
            var threeJoltDiffs = 1; // built-in adapter is +3 jolts
            var currentIndex = 0;

            while (currentIndex < joltages.Count - 1)
            {
                Console.WriteLine(joltages[currentIndex]);
                var diff = joltages[currentIndex + 1] - joltages[currentIndex];
                if (diff == 1)
                {
                    oneJoltDiffs++;
                }
                else if (diff == 3)
                {
                    threeJoltDiffs++;
                }
                else
                {
                    Console.WriteLine("unexpected diff of {0}!", diff);
                }
                currentIndex++;
            }

            Console.WriteLine("1 jolt diffs: {0}", oneJoltDiffs);
            Console.WriteLine("3 jolt diffs: {0}", threeJoltDiffs);
            Console.WriteLine(oneJoltDiffs * threeJoltDiffs);

            Console.WriteLine(ProcessJoltageIndex(joltages, 0));
        }

        

        private static long ProcessJoltageIndex(List<int> joltages, int currentIndex)
        {
            // if (cache.ContainsKey(currentIndex))
            // {
            //     Console.WriteLine("cache entry found for " + currentIndex);
            //     return cache[currentIndex];
            // }

            if (currentIndex >= joltages.Count)
            {
                // Console.WriteLine("done with this tree for index " + currentIndex);
                return 1;
            }

            // Console.WriteLine("current index " + currentIndex);

            var currentNumber = joltages[currentIndex];
            long result = ProcessJoltageIndex(joltages, currentIndex + 1);
            // if (!cache.ContainsKey(currentIndex + 1))
            // {
            //     cache.Add(currentIndex, result);
            // }
            
            // Console.WriteLine(result);

            if (currentIndex + 2 < joltages.Count && joltages[currentIndex + 2] - currentNumber <= 3)
            {
                // Console.WriteLine(joltages[currentIndex + 2] + " - " + currentNumber);
                // Console.WriteLine("adding val 2 spots away from " + currentIndex);
                result += ProcessJoltageIndex(joltages, currentIndex + 2);
            }

            if (currentIndex + 3 < joltages.Count && joltages[currentIndex + 3] - currentNumber <= 3)
            {
                // Console.WriteLine(joltages[currentIndex + 3] + " - " + currentNumber);
                // Console.WriteLine("adding val 3 spots away from " + currentIndex);
                result += ProcessJoltageIndex(joltages, currentIndex + 3);
            }            

            return result;
        }
    }
}
