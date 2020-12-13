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
                // Console.WriteLine(joltages[currentIndex]);
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
            
            // need to add the 0 value to the beginning of the array to correctly calculate all the paths
            joltages.Insert(0, 0);
            Console.WriteLine(ProcessJoltageIndex(joltages, 0));
        }        

        private static long ProcessJoltageIndex(List<int> joltages, int currentIndex)
        {
            if (currentIndex == joltages.Count - 1)
            {
                return 1;
            }

            if (cache.ContainsKey(currentIndex))
            {
                return cache[currentIndex];
            }

            long result = 0;
            for (int i = currentIndex + 1; i < joltages.Count; i++)
            {
                if (joltages[i] - joltages[currentIndex] <= 3)
                {
                    result += ProcessJoltageIndex(joltages, i);
                }
            }

            cache.TryAdd(currentIndex, result);

            return result;
        }
    }
}
