using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Problem2
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

            var parsedInputs = new List<PasswordPolicyContainer>();

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    parsedInputs.Add(new PasswordPolicyContainer(fileStream.ReadLine()));
                }
            }

            var passingPasswords = parsedInputs.Count(x => x.PasswordMeetsPolicy());
            Console.WriteLine(passingPasswords);
        }
    }

    class PasswordPolicyContainer
    {
        public PasswordPolicyContainer(string input)
        {
            ParseInput(input);
        }

        public char PolicyLetter { get; private set; }
        public int MinimumOccurrences { get; private set; }
        public int MaximumOccurrences { get; private set; }
        public string Password { get; private set; }

        public bool PasswordMeetsPolicy()
        {
            var policyLetterFound = Password.GroupBy(x => x)
                .Select(x => new {
                    Letter = x.Key,
                    Occurrences = x.Count()
                })
                .ToDictionary(x => x.Letter, x => x.Occurrences)
                .TryGetValue(PolicyLetter, out var occurrencesForPolicyLetter);
            
            return policyLetterFound 
                    && occurrencesForPolicyLetter >= MinimumOccurrences 
                    && occurrencesForPolicyLetter <= MaximumOccurrences;
        }

        private void ParseInput(string input)
        {
            var splitInput = input.Split(' ');
            var ranges = splitInput[0].Split('-');
            MinimumOccurrences = Convert.ToInt32(ranges[0]);
            MaximumOccurrences = Convert.ToInt32(ranges[1]);
            PolicyLetter = splitInput[1][0];
            Password = splitInput[2];
        }
    }
}
