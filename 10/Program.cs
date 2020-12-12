using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AocSolution
{
    class Program
    {
        static Dictionary<long, int> cache = new Dictionary<long, int>();

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

            // Console.WriteLine(CountAdapterPermutations(joltages);
        }

        private static long CountAdapterPermutations(List<int> joltages, int currentIndex)
        {
            long result = 0;
            var currentNumber = joltages[currentIndex];
            var offset = currentIndex + 1;
            var diffExceeded = false;
            while (!diffExceeded)
            {
                if (joltages[offset] - joltages[currentIndex] <= 3)
                {
                    result += ProcessJoltageIndex(joltages, offset);
                    offset++;
                }
                else
                {
                    diffExceeded = true;
                }
            }
        }

        private static long ProcessJoltageIndex(List<int> joltages, int currentIndex)
        {
            // if (cache.Contains())
        }
    }
}
