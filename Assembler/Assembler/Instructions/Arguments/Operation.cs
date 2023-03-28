namespace Assembler.Instructions.Arguments
{
    internal class Operation : IArgument
    {
        private byte value;

        public bool Test(string s)
        {
            switch (s.ToUpper())
            {
                case "ADD":
                    value = 0x00;
                    break;
                case "SUB":
                    value = 0x01;
                    break;
                case "MULT":
                    value = 0x02;
                    break;
                case "ROT":
                    value = 0x03;
                    break;
                case "NOT":
                    value = 0x04;
                    break;
                case "AND":
                    value = 0x05;
                    break;
                case "OR":
                    value = 0x06;
                    break;
                case "XOR":
                    value = 0x07;
                    break;
                case "DIV":
                    value = 0x03;
                    break;
                case "FLT":
                    value = 0x04;
                    break;
                case "INT":
                    value = 0x05;
                    break;
                default:
                    return false;
            }
            return true;
        }

        public void Load(string s)
        {
            if (!Test(s))
            {
                throw new ArgumentException($"Invalid operation: {s}");
            }
        }

        public byte Value
        {
            get { return value; }
        }

        public ArgumentType Type => ArgumentType.Operation;
    }
}