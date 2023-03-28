using Assembler.Instructions.Arguments;

namespace Assembler.Instructions;
internal class Parser
{
    public Instruction GetInstruction(string str)
    {
        var (name, args) = ParseFirstWordAndArguments(str);

        return new Instruction
        {
            Name = name,
            Arguments = args.Select(arg => ParseArgument(arg)).ToArray()
        };
    }

    private (string, string[]) ParseFirstWordAndArguments(string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            return (string.Empty, new string[0]);
        }

        var words = inputString.Trim().Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length > 0)
        {
            var firstWord = words[0].ToUpper();
            var arguments = new string[words.Length - 1];
            for (var i = 1; i < words.Length; i++)
            {
                arguments[i - 1] = words[i].Trim();
            }
            return (firstWord, arguments);
        }
        else
        {
            throw new Exception("Invalid command");
        }
    }

    private IArgument ParseArgument(string arg)
    {
        var argumentTypes = new IArgument[]
        {
            new Condition(),
            new Label(),
            new Literal(),
            new Operation(),
            new Register(),
        };

        foreach(var argument in argumentTypes)
        {
            if (argument.Test(arg))
            {
                argument.Load(arg);
                return argument;
            }
        }

        throw new Exception("Invalid Argument");
    }
}

