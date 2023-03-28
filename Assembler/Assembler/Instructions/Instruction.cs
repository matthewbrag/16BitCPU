using Assembler.Instructions.Arguments;

internal class Instruction
{
    public string Name { get; init;  }
    public IArgument[] Arguments { get; init; }

    public string GetString()
    {
        return Name  + " " + string.Join(", ", Arguments.Select(arg => arg.Type.ToString()));
    }
}