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

            var parsedInputs = new List<BasePasswordPolicy>();

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    if (args[1] == "1")
                    {
                        parsedInputs.Add(new PasswordRangeOccurrencePolicy(fileStream.ReadLine()));
                    }
                    else if (args[1] == "2")
                    {
                        parsedInputs.Add(new PasswordIndexOccurrencePolicy(fileStream.ReadLine()));
                    }
                    else
                    {
                        Console.WriteLine("Provide problem part (1 or 2).");
                        return;
                    }
                }
            }

            var passingPasswords = parsedInputs.Count(x => x.PasswordMeetsPolicy());
            Console.WriteLine(passingPasswords);
        }
    }

    abstract class BasePasswordPolicy
    {
        public BasePasswordPolicy(string input)
        {
            ParseInput(input);
        }

        public char PolicyLetter { get; protected set; }
        public string Password { get; protected set; }
        public int Parameter1 { get; protected set; }
        public int Parameter2 { get; protected set; }

        private void ParseInput(string input)
        {            
            var splitInput = input.Split(' ');
            var ranges = splitInput[0].Split('-');
            Parameter1 = Convert.ToInt32(ranges[0]);
            Parameter2 = Convert.ToInt32(ranges[1]);
            PolicyLetter = splitInput[1][0];
            Password = splitInput[2];
        }

        public abstract bool PasswordMeetsPolicy();
    }

    class PasswordIndexOccurrencePolicy : BasePasswordPolicy
    {
        public PasswordIndexOccurrencePolicy(string input) : base(input) { }

        public override bool PasswordMeetsPolicy()
        {
            return Password[Parameter1 - 1] == PolicyLetter ^ Password[Parameter2 - 1] == PolicyLetter;
        }
    }

    class PasswordRangeOccurrencePolicy : BasePasswordPolicy
    {
        public PasswordRangeOccurrencePolicy(string input) : base(input) { }        

        public override bool PasswordMeetsPolicy()
        {
            var policyLetterFound = Password.GroupBy(x => x)
                .Select(x => new {
                    Letter = x.Key,
                    Occurrences = x.Count()
                })
                .ToDictionary(x => x.Letter, x => x.Occurrences)
                .TryGetValue(PolicyLetter, out var occurrencesForPolicyLetter);
            
            return policyLetterFound 
                    && occurrencesForPolicyLetter >= Parameter1 
                    && occurrencesForPolicyLetter <= Parameter2;
        }        
    }
}
