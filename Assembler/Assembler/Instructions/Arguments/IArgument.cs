namespace Assembler.Instructions.Arguments;
internal interface IArgument
{
    public bool Test(string s);
    public void Load(string s);

    public ArgumentType Type { get; }
}
