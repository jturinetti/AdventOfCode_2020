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

            var graph = new BagGraph();

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    var node = BagNode.ParseBagInput(fileStream.ReadLine());
                    graph.AddNode(node);
                }
            }

            Console.WriteLine(graph.BagTraversal("shiny gold"));
        }
    }

    class BagGraph
    {
        private Dictionary<string, BagNode> nodeDict { get; } = new Dictionary<string, BagNode>();

        public void AddNode(BagNode node)
        {
            nodeDict.Add(node.BagName, node);
        }

        public int BagTraversal(string bagName)
        {
            return nodeDict.Keys.Where(bagKey => BagTraversalRec(nodeDict[bagKey], bagName)).Count();
        }

        private bool BagTraversalRec(BagNode currentNode, string bagName)
        {
            if (!currentNode.ContainedBags.Any())
            {
                return false;
            }

            return currentNode.ContainedBags.Any(b => b.BagName == bagName) 
                || currentNode.ContainedBags.Any(b => BagTraversalRec(nodeDict[b.BagName], bagName));
        }
    }

    class BagNode
    {
        public string BagName { get; set; }
        public int BagCount { get; set; } = 1;

        public List<BagNode> ContainedBags { get; } = new List<BagNode>();

        public static BagNode ParseBagInput(string inputLine)
        {
            var retNode = new BagNode();

            var splitInput = inputLine.Split(' ');
            retNode.BagName = splitInput[0] + " " + splitInput[1];
            var inputIndex = 4;

            while (inputIndex <= splitInput.Length - 4 && splitInput[inputIndex] != "no")
            {
                retNode.ContainedBags.Add(new BagNode
                {
                    BagCount = Convert.ToInt32(splitInput[inputIndex]),
                    BagName = splitInput[inputIndex + 1] + " " + splitInput[inputIndex + 2]
                });
                
                inputIndex = inputIndex + 4;
            }

            return retNode;
        }

        public override string ToString()
        {
            var str = "";
            str += "Bag name: " + BagName + "\n";
            if (ContainedBags.Any())
            {
                str += "Contained Bags:\n";
                foreach (var bag in ContainedBags)
                {
                    str += "\t" + bag.BagCount + " " + bag.BagName + " bag(s)";
                }
            }
            
            return str;
        }
    }
}
