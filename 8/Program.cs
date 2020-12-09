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

            var bp = new BootProgram();            

            using (StreamReader fileStream = new StreamReader(new FileInfo(args[0]).OpenRead()))
            {
                while (fileStream.Peek() >= 0)
                {
                    bp.ParseInputInstruction(fileStream.ReadLine());
                }
            }

            // part 1
            Console.WriteLine("[part 1]");
            bp.RunProgram();
            Console.WriteLine(bp.Accumulator);

            Console.WriteLine();

            // part 2
            Console.WriteLine("[part 2]");
            for (var index = 0; index < bp.Instructions.Count - 1; index++)
            {
                var instruction = bp.Instructions[index];
                if (instruction.Command != BootProgram.NOOP && instruction.Command != BootProgram.JUMP)
                {
                    continue;
                }  

                List<Instruction> updatedInstructions = null;
                if (instruction.Command == BootProgram.NOOP)
                {
                    updatedInstructions = CopyAndModifyInstructionSet(bp.Instructions, BootProgram.JUMP, index);
                }
                else if (instruction.Command == BootProgram.JUMP)
                {
                    updatedInstructions = CopyAndModifyInstructionSet(bp.Instructions, BootProgram.NOOP, index);
                }

                var updatedProgram = new BootProgram(updatedInstructions);
                updatedProgram.RunProgram();
                if (updatedProgram.DidTerminate)
                {
                    Console.WriteLine(updatedProgram.Accumulator);
                    break;
                }
            }
        }

        private static List<Instruction> CopyAndModifyInstructionSet(List<Instruction> originalInstructions, string updatedCommand, int index)
        {
            var copiedInstructions = new List<Instruction>(originalInstructions);
            var newInstruction = new Instruction(updatedCommand, originalInstructions[index].Symbol, originalInstructions[index].Parameter);
            copiedInstructions.RemoveAt(index);
            copiedInstructions.Insert(index, newInstruction);
            return copiedInstructions;
        }
    }

    class BootProgram
    {
        #region Known Commands
        public const string NOOP = "nop";
        public const string JUMP = "jmp";
        public const string ACCUMULATE = "acc";
        #endregion

        public BootProgram() { }

        public BootProgram(List<Instruction> instructions)
        {
            Instructions = instructions;
        }

        public List<Instruction> Instructions { get; } = new List<Instruction>();
        public int Accumulator = 0;
        public bool DidTerminate { get; private set; } = false;
        private HashSet<int> processedInstructions = new HashSet<int>();
        private int instructionIndex = 0;

        public void ParseInputInstruction(string inputLine)
        {
            var splitInput = inputLine.Split(' ');
            Instructions.Add(new Instruction(splitInput[0], splitInput[1][0], Convert.ToInt32(splitInput[1].Substring(1))));
        }

        public void RunProgram()
        {
            while (!InfiniteLoopDetected() && !ProgramTerminated())
            {
                var instructionDelta = 0;
                switch (Instructions[instructionIndex].Command) {
                    case ACCUMULATE:
                        Accumulator += Instructions[instructionIndex].SignedParameter;
                        instructionDelta = 1;
                        break;
                    case JUMP:
                        instructionDelta = Instructions[instructionIndex].SignedParameter;
                        break;
                    case NOOP:
                    default:
                        instructionDelta = 1;
                        break;
                };

                StoreProcessedInstructionIndex(instructionIndex);
                instructionIndex += instructionDelta;
            }
        }

        private bool InfiniteLoopDetected()
        {
            if (processedInstructions.Contains(instructionIndex))
            {
                Console.WriteLine("INFINITE LOOP DETECTED (instruction {0} seen again)", instructionIndex);
                return true;
            }

            return false;
        }

        private bool ProgramTerminated()
        {
            if (instructionIndex == Instructions.Count)
            {
                Console.WriteLine("PROGRAM TERMINATED!");
                DidTerminate = true;
                return true;
            }
            
            return false;
        }

        private void StoreProcessedInstructionIndex(int instructionIndex)
        {
            processedInstructions.Add(instructionIndex);
        }
    }

    class Instruction
    {
        public Instruction(string command, char symbol, int parameter)
        {
            Command = command;
            Symbol = symbol;
            Parameter = parameter;
        }

        public string Command { get; }
        public char Symbol { get; }
        public int Parameter { get; }
        public int SignedParameter => Symbol switch
        {
            '-' => -1 * Parameter,
            _ => Parameter
        };
    }
}
