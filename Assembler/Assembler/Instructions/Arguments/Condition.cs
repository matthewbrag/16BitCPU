namespace Assembler.Instructions.Arguments
{
    internal class Condition : IArgument
    {
        private byte value;

        public bool Test(string s)
        {
            switch (s.ToUpper())
            {
                case "NEG":
                case "POS":
                case "ZERO":
                case "OVF":
                case "FNEG":
                case "FPOS":
                case "FZERO":
                case "FERR":
                    return true;
                default:
                    return false;
            }
        }

        public void Load(string s)
        {
            if (!Test(s))
            {
                throw new ArgumentException($"Invalid condition: {s}");
            }

            switch (s.ToUpper())
            {
                case "ZERO":
                    value = 0x00;
                    break;
                case "POS":
                    value = 0x01;
                    break;
                case "NEG":
                    value = 0x02;
                    break;
                case "OVF":
                    value = 0x03;
                    break;
                case "FZERO":
                    value = 0x04;
                    break;
                case "FPOS":
                    value = 0x05;
                    break;
                case "FNEG":
                    value = 0x06;
                    break;
                case "FERR":
                    value = 0x07;
                    break;
            }
        }

        public byte Value
        {
            get { return value; }
        }

        public ArgumentType Type => ArgumentType.Condition;
    }
}