using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
                        
                        var inputList = ParsePassportInput(passportLine);
                        passport.AddPassportFields(inputList);
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

        private static List<PassportField> ParsePassportInput(string line)
        {
            var retList = new List<PassportField>();
            var parseLineEntries = line.Split(' ');
            foreach (var entry in parseLineEntries)
            {
                var kvp = entry.Split(':');
                retList.Add(new PassportField(kvp[0], kvp[1]));
            }
            return retList;
        }
    }

    class PassportField
    {
        public string FieldKey { get; }
        public string FieldValue { get; }
        public bool IsValid { get; private set; }
        private PassportFieldValidatorFactory validatorFactory;

        public PassportField(string key, string value)
        {
            FieldKey = key;
            FieldValue = value;

            validatorFactory = new PassportFieldValidatorFactory();
            IsValid = validatorFactory.CreateValidator(key).IsPassportFieldValid(value);
        }
    }

    class PassportFieldValidatorFactory
    {
        public IPassportFieldValidator CreateValidator(string fieldKey)
        {
            switch (fieldKey)
            {
                case "byr":
                    return new ByrValidator();
                case "iyr":
                    return new IyrValidator();
                case "eyr":
                    return new EyrValidator();
                case "hgt":
                    return new HgtValidator();
                case "hcl":
                    return new HclValidator();
                case "ecl":
                    return new EclValidator();
                case "pid":
                    return new PidValidator();
                default:
                    return new DefaultValidator();
            }
        } 
    }

    interface IPassportFieldValidator
    {
        bool IsPassportFieldValid(string value);
    }

    class DefaultValidator : IPassportFieldValidator
    {
        public bool IsPassportFieldValid(string value)
        {
            return true;
        }
    }

    class ByrValidator : IPassportFieldValidator
    {
        public bool IsPassportFieldValid(string value)
        {
            return value.Length == 4 
                && int.TryParse(value, out int year) 
                && year >= 1920 
                && year <= 2002;
        }
    }

    class IyrValidator : IPassportFieldValidator
    {
        public bool IsPassportFieldValid(string value)
        {
            return value.Length == 4 
                && int.TryParse(value, out int year) 
                && year >= 2010 
                && year <= 2020;
        }
    }

    class EyrValidator : IPassportFieldValidator
    {
        public bool IsPassportFieldValid(string value)
        {
            return value.Length == 4 
                && int.TryParse(value, out int year) 
                && year >= 2020 
                && year <= 2030;
        }
    }

    class HgtValidator : IPassportFieldValidator
    {
        public bool IsPassportFieldValid(string value)
        {
            var parsedUnit = value.Substring(value.Length - 2);
            var parsedHeight = value.Substring(0, value.Length - 2);

            if (parsedUnit == "cm")
            {
                return int.TryParse(parsedHeight, out int height) && height >= 150 && height <= 193;
            }
            else if (parsedUnit == "in")
            {
                return int.TryParse(parsedHeight, out int height) && height >= 59 && height <= 76;
            }

            return false;
        }
    }

    class HclValidator : IPassportFieldValidator
    {
        public bool IsPassportFieldValid(string value)
        {
            return Regex.IsMatch(value, "#([0-9a-f]){6}");
        }
    }
    
    class EclValidator : IPassportFieldValidator
    {
        public bool IsPassportFieldValid(string value)
        {
            return Regex.IsMatch(value, "amb|blu|brn|gry|grn|hzl|oth");
        }
    }

    class PidValidator : IPassportFieldValidator
    {
        public bool IsPassportFieldValid(string value)
        {
            return value.Length == 9 && int.TryParse(value, out _);
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
            // "cid"    this field is optional
        };

        private readonly List<PassportField> passportFields = new List<PassportField>();

        public void AddPassportFields(List<PassportField> fields)
        {
            passportFields.AddRange(fields);
        }

        public bool IsPassportValid()
        {
            var isValid = true;
            foreach (var field in requiredPassportFields)
            {
                isValid = isValid 
                    && passportFields.Exists(f => f.FieldKey == field)
                    && passportFields.Single(f => f.FieldKey == field).IsValid;
            }
            return isValid;
        }

        public override string ToString()
        {
            var str = "";
            foreach (var field in passportFields)
            {
                str += string.Format("{0} : {1}\n", field.FieldKey, field.FieldValue);
            }
            return str;
        }
    }
}
