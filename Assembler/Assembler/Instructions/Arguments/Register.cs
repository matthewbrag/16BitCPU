using System.Text.RegularExpressions;

namespace Assembler.Instructions.Arguments;

internal class Register : IArgument
{
    private byte value;

    public bool Test(string s)
    {
        return Regex.IsMatch(s, @"^[Rr]_?[0-9A-Fa-f]$");
    }

    public void Load(string s)
    {
        if (!Test(s))
        {
            throw new ArgumentException($"Invalid register: {s}");
        }

        var hexString = s.Substring(s.Length - 1);
        value = Convert.ToByte(hexString, 16);
    }

    public byte Value
    {
        get { return value; }
    }

    public ArgumentType Type => ArgumentType.Register;
}
