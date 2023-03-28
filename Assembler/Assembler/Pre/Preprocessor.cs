using System.Text.RegularExpressions;

namespace Assembler.Pre;
internal class Preprocessor
{
    private static readonly Regex commentRegex = new Regex(@"(?:/|;).*?$", RegexOptions.Multiline);

    public string[] Preprocess(string input)
    {
        input = commentRegex.Replace(input, string.Empty);

        var instructions = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < instructions.Length; i++)
        {
            instructions[i] = instructions[i].Trim();
        }

        instructions = instructions.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        return instructions;
    }
}