using Assembler.Instructions.Arguments;

namespace Assembler.Instructions;
internal class InstructionSelector
{
    public InstructionSelector(Instruction instruction)
    {
        Name = instruction.Name;
        Types = instruction.Arguments.Select(arg => arg.Type).ToArray();
    }

    public InstructionSelector(string name, ArgumentType[] types)
    {
        Name = name;
        Types = types;
    }

    public string Name { get; set; }
    public ArgumentType[] Types { get; set; }


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (InstructionSelector)obj;
        return Name == other.Name && Types.SequenceEqual(other.Types);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + Name.GetHashCode();
            foreach (var argType in Types)
            {
                hash = hash * 23 + argType.GetHashCode();
            }
            return hash;
        }
    }
}
