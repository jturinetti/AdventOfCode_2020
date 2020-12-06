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

            var groups = new List<Group>();

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    var group = new Group();
                    while (true)
                    {
                        var singlePersonResponses = fileStream.ReadLine();                        
                        if (string.IsNullOrWhiteSpace(singlePersonResponses))
                        {
                            Console.WriteLine("Question Count: {0}", group.QuestionsAnsweredYesByAtLeastOnePerson);
                            groups.Add(group);
                            break;
                        }

                        group.QuestionResponses.Add(singlePersonResponses);
                    }
                }

                Console.WriteLine("Total Group Count: {0}", groups.Count);
                Console.WriteLine("Total Questions Answered Yes By At Least One Person: {0}", groups.Sum(g => g.QuestionsAnsweredYesByAtLeastOnePerson));
                Console.WriteLine("Total Questions Answered Yes By Everyone In Group: {0}", groups.Sum(g => g.QuestionsAnsweredYesByEveryone));
            }
        }
    }

    class Group
    {
        public Group()
        {
            QuestionResponses = new List<string>();
        }

        public List<string> QuestionResponses { get; }
        public int NumberOfPeople => QuestionResponses.Count;
        public int QuestionsAnsweredYesByAtLeastOnePerson => string.Join("", QuestionResponses)
                                                                .Distinct()
                                                                .Count();
        public int QuestionsAnsweredYesByEveryone => string.Join("", QuestionResponses)
                                                        .GroupBy(x => x)
                                                        .Select(x => new { Response = x.Key, ResponseCount = x.Count()})
                                                        .Count(x => x.ResponseCount == NumberOfPeople);
    }
}
