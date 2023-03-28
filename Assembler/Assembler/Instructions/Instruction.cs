using Assembler.Instructions.Arguments;

internal class Instruction
{
    public string Name { get; init;  }
    public IArgument[] Arguments { get; init; }
}