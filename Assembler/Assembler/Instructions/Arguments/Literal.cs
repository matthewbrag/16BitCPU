using System.Text.RegularExpressions;

namespace Assembler.Instructions.Arguments;
internal class Literal : IArgument
{

    public short Value { get; private set; }

    public ArgumentType Type => ArgumentType.Literal;

    public bool Test(string s)
    {
        return Regex.IsMatch(s, @"^(0b[01]{16}|0x[0-9A-Fa-f]{4}|-?\d+|(0f-?)\d+\.\d*|(0s-?)\d+\.\d*)$");
    }

    public void Load(string s)
    {
        if (!Test(s))
        {
            throw new ArgumentException($"Invalid literal: {s}");
        }

        if (s.StartsWith("0b"))
        {
            // Binary literal
            var binaryString = s.Substring(2);
            if (binaryString[0] == '1')
            {
                // Negative number
                Value = (short)(Convert.ToInt16(binaryString, 2) - 0x10000);
            }
            else
            {
                // Positive number
                Value = Convert.ToInt16(binaryString, 2);
            }
        }
        else if (s.StartsWith("0x"))
        {
            // Hex literal
            var hexString = s.Substring(2);
            if (hexString[0] >= '8')
            {
                // Negative number
                Value = (short)(Convert.ToInt16(hexString, 16) - 0x10000);
            }
            else
            {
                // Positive number
                Value = Convert.ToInt16(hexString, 16);
            }
        }
        else if (s.StartsWith("-"))
        {
            // Decimal negative literal
            Value = Convert.ToInt16(s);
        }
        else if (s.StartsWith("0f"))
        {
            // Floating point first half literal
            var floatValue = Convert.ToSingle(s.Substring(2));
            var bitString = FloatToBits(floatValue);
            var shortValue = Convert.ToInt16(bitString[0..16], 2);
            Value = shortValue;
        }
        else if (s.StartsWith("0s"))
        {
            // Floating point second half literal
            var floatValue = Convert.ToSingle(s.Substring(2));
            var bitString = FloatToBits(floatValue);
            var shortValue = Convert.ToInt16(bitString[16..32], 2);
            Value = shortValue;
        }
        else
        {
            // Decimal positive literal
            Value = Convert.ToInt16(s);
        }
    }

    private string FloatToBits(float value)
    {
        return string.Join("", BitConverter.GetBytes(value)
            .Reverse()
            .Select(x => Convert.ToString(x, 2))
            .Select(x => x.PadLeft(8, '0')));
    }
}
