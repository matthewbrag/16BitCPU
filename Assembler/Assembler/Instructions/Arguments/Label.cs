using System.Text.RegularExpressions;

namespace Assembler.Instructions.Arguments
{
    internal class Label : IArgument
    {
        private string value;

        public bool Test(string s)
        {
            var registerArg = new Register();

            return !Regex.IsMatch(s, @"^\d") && !IsReservedWord(s) && ! registerArg.Test(s);
        }

        public void Load(string s)
        {
            if (!Test(s))
            {
                throw new ArgumentException($"Invalid label: {s}");
            }

            value = s;
        }

        public string Name
        {
            get { return value; }
        }

        public ArgumentType Type => ArgumentType.Label;

        private static bool IsReservedWord(string s)
        {
            var reservedWords = new string[] {
                "CONT",
                "HLT",
                "LABEL",
                "LD",
                "MOV",
                "INP",
                "OUT", 
                "GPSET",
                "GPLD",
                "INOP",
                "FLOP",
                "JMP",
                "JCN",
                "ADD",
                "SUB",
                "MULT",
                "ROT",
                "DIV",
                "AND",
                "NOT",
                "OR",
                "XOR",
                "FLT",
                "INT",
                "NEG",
                "POS",
                "ZERO",
                "OVF",
                "FNEG",
                "FPOS",
                "FZERO",
                "FERR"
            };

            return reservedWords.Contains(s);
        }
    }

}