using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Problem4
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

            var validPassportCount = 0;

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    // new passport starting
                    var passport = new Passport();
                    while (true)
                    {
                        var passportLine = fileStream.ReadLine();
                        if (string.IsNullOrWhiteSpace(passportLine))
                        {
                            // end of passport lines
                            break;
                        }
                        
                        var inputDictionary = ParsePassportInput(passportLine);                        
                        passport.AddPassportFields(inputDictionary);
                    }

                    Console.WriteLine(passport);

                    if (passport.IsPassportValid())
                    {
                        validPassportCount++;
                    }
                }
            }

            Console.WriteLine("Valid passports: {0}", validPassportCount);
        }

        private static Dictionary<string, string> ParsePassportInput(string line)
        {
            var retDictionary = new Dictionary<string, string>();
            var parseLineEntries = line.Split(' ');
            foreach (var entry in parseLineEntries)
            {
                var kvp = entry.Split(':');
                retDictionary.Add(kvp[0], kvp[1]);
            }
            return retDictionary;
        }
    }

    class Passport
    {
        private readonly List<string> requiredPassportFields = new List<string>
        {
            "byr",
            "iyr",
            "eyr",
            "hgt",
            "hcl",
            "ecl",
            "pid",
            // "cid"    this field can be missing
        };

        private readonly Dictionary<string, string> passportFields = new Dictionary<string, string>();

        public void AddPassportField(string key, string value)
        {
            passportFields.Add(key, value);
        }

        public void AddPassportFields(Dictionary<string, string> fields)
        {            
            foreach (var field in fields)
            {
                passportFields.Add(field.Key, field.Value);
            }            
        }

        public bool IsPassportValid()
        {
            var isValid = true;
            foreach (var field in requiredPassportFields)
            {
                isValid = isValid && passportFields.ContainsKey(field);
            }
            return isValid;
        }

        public override string ToString()
        {
            var str = "";
            foreach (var field in passportFields)
            {
                str += string.Format("{0} : {1}\n", field.Key, field.Value);
            }
            return str;
        }
    }
}
