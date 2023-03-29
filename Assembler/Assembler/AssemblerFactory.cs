using Assembler.Instructions;
using Assembler.Instructions.Arguments;

namespace Assembler;
internal class AssemblerFactory
{
    private readonly Dictionary<string, string> _labelMappings;

    public AssemblerFactory()
    {
        _labelMappings = new Dictionary<string, string>();
    }

    public AssemblerFactory(Dictionary<string, string> labelMappings)
    {
        _labelMappings = labelMappings;
    }

    public string[] GetHex(Instruction instruction, ushort pc)
    {
        var selector = new InstructionSelector(instruction);

        if (_labelDict.ContainsKey(selector))
        {
            _labelDict[selector](instruction, pc, _labelMappings);
            return new string[0];
        }
        else if(_singleWordDict.ContainsKey(selector))
        {
            return new string[] { _singleWordDict[selector](instruction, _labelMappings) };
        }
        else if (_dualWordDict.ContainsKey(selector))
        {
            var (word1, word2) = _dualWordDict[selector](instruction, _labelMappings);
            return new string[] { word1, word2 };
        }
        else
        {
            throw new Exception("Unrecognized Opertaion: "  + instruction.GetString());
        }
            
    }

    public Dictionary<string, string> GetMappings()
    {
        return _labelMappings;
    }

    private readonly Dictionary<InstructionSelector, Action<Instruction,ushort, Dictionary<string, string>>> _labelDict = new Dictionary<InstructionSelector, Action<Instruction,ushort, Dictionary<string, string>>>()
    {
        {new InstructionSelector("LABEL", new ArgumentType[]{ArgumentType.Label}), (ins, pc, mappings) =>
            {
                var lab = ins.Arguments[0] as Label;
                if(mappings.ContainsKey(lab.Name))
                    mappings[lab.Name] = ToHex(pc);
                else
                    mappings.Add(lab.Name, ToHex(pc));
            }
        }
    };

    private readonly Dictionary<InstructionSelector, Func<Instruction, Dictionary<string, string>, string>> _singleWordDict = new Dictionary<InstructionSelector, Func<Instruction, Dictionary<string, string>, string>>()
    {
        {new InstructionSelector("CONT", new ArgumentType[]{}),(ins, mappings) => "0000"},
        {new InstructionSelector("HLT", new ArgumentType[]{}),(ins, mappings) => "0001"},
        {new InstructionSelector("MOV", new ArgumentType[]{ArgumentType.Register, ArgumentType.Register}),(ins, mappings) =>
            {
                var fromReg = ins.Arguments[0] as Register;
                var toReg = ins.Arguments[1] as Register;
                return $"20{ToHex(fromReg)}{ToHex(toReg)}";
            }
        },
        {new InstructionSelector("INP", new ArgumentType[]{ArgumentType.Literal, ArgumentType.Register}),(ins, mappings) =>
            {
                var inp = ins.Arguments[0] as Literal;

                if(inp.Value < 0 || inp.Value > 3)
                    throw new Exception("Input out of range");

                var toReg = ins.Arguments[1] as Register;
                return $"3{ToHexSingle(inp)}0{ToHex(toReg)}";
            }
        },
        {new InstructionSelector("OUT", new ArgumentType[]{ArgumentType.Literal, ArgumentType.Register}),(ins, mappings) =>
            {
                var outSel = ins.Arguments[0] as Literal;

                if(outSel.Value < 0 || outSel.Value > 3)
                    throw new Exception("Input out of range");

                var toReg = ins.Arguments[1] as Register;
                return $"3{outSel.Value + 4:X}0{ToHex(toReg)}";
            }
        },
        {new InstructionSelector("GPSET", new ArgumentType[]{ArgumentType.Register, ArgumentType.Register}),(ins, mappings) =>
            {
                var addr = ins.Arguments[0] as Register;
                var reg = ins.Arguments[1] as Register;
                return $"40{ToHex(addr)}{ToHex(reg)}";
            }
        },
        {new InstructionSelector("GPLD", new ArgumentType[]{ArgumentType.Register, ArgumentType.Register}),(ins, mappings) =>
            {
                var addr = ins.Arguments[0] as Register;
                var reg = ins.Arguments[1] as Register;
                return $"41{ToHex(addr)}{ToHex(reg)}";
            }
        },
        {new InstructionSelector("FLOP", new ArgumentType[]{ArgumentType.Operation}),(ins, mappings) =>
            {
                var op = ins.Arguments[0] as Operation;
                return $"9{ToHex(op)}00";
            }
        }
    };

    private readonly Dictionary<InstructionSelector, Func<Instruction, Dictionary<string, string>, (string, string)>> _dualWordDict = new Dictionary<InstructionSelector, Func<Instruction, Dictionary<string, string>, (string, string)>>()
    {
        {
            new InstructionSelector("LD", new ArgumentType[]{ArgumentType.Literal, ArgumentType.Register}), (inst, mappings) =>
            {
                var reg = inst.Arguments[1] as Register;
                var literal = inst.Arguments[0] as Literal;
                return ($"100{ToHex(reg)}", ToHex(literal));
            }
        },
        {
            new InstructionSelector("INOP", new ArgumentType[]{ArgumentType.Operation, ArgumentType.Register, ArgumentType.Register, ArgumentType.Register}), (inst, mappings) =>
            {
                var op = inst.Arguments[0] as Operation;
                var regResult = inst.Arguments[1] as Register;
                var rega = inst.Arguments[2] as Register;
                var regb = inst.Arguments[3] as Register;

                return ($"6{ToHex(op)}{ToHex(regResult)}{ToHex(rega)}", $"000{ToHex(regb)}");
            }
        },
        {
            new InstructionSelector("INOP", new ArgumentType[]{ArgumentType.Operation, ArgumentType.Register, ArgumentType.Register, ArgumentType.Literal}), (inst, mappings) =>
            {
                var op = inst.Arguments[0] as Operation;
                var regResult = inst.Arguments[1] as Register;
                var rega = inst.Arguments[2] as Register;
                var lit = inst.Arguments[3] as Literal;

                return ($"7{ToHex(op)}{ToHex(regResult)}{ToHex(rega)}", ToHex(lit));
            }
        },
        {
            new InstructionSelector("INOP", new ArgumentType[]{ArgumentType.Operation, ArgumentType.Register, ArgumentType.Literal, ArgumentType.Register}), (inst, mappings) =>
            {
                var op = inst.Arguments[0] as Operation;
                var regResult = inst.Arguments[1] as Register;
                var rega = inst.Arguments[3] as Register;
                var lit = inst.Arguments[2] as Literal;

                return ($"7{op.Value + 8:X}{ToHex(regResult)}{ToHex(rega)}", ToHex(lit));
            }
        },
        {
            new InstructionSelector("JMP", new ArgumentType[]{ArgumentType.Label}), (inst, mappings) =>
            {
                var label = inst.Arguments[0] as Label;
                var jumpAddr = mappings.ContainsKey(label.Name) ? mappings[label.Name] : "UUUU";

                return ($"A000", jumpAddr);
            }
        },
        {
            new InstructionSelector("JCN", new ArgumentType[]{ArgumentType.Condition, ArgumentType.Label}), (inst, mappings) =>
            {
                var con = inst.Arguments[0] as Condition;
                var label = inst.Arguments[1] as Label;
                var jumpAddr = mappings.ContainsKey(label.Name) ? mappings[label.Name] : "UUUU";

                return ($"A1{ToHex(con)}0", jumpAddr);
            }
        },

    };

    private static string ToHex(ushort val)
    {
        return val.ToString("X4");
    }
    private static string ToHex(Register register)
    {
        return register.Value.ToString("X");
    }
    private static string ToHex(Literal literal)
    {
        return literal.Value.ToString("X4");
    }
    private static string ToHexSingle(Literal literal)
    {
        return literal.Value.ToString("X");
    }
    private static string ToHex(Operation operation)
    {
        return operation.Value.ToString("X");
    }
    private static string ToHex(Condition condition)
    {
        return condition.Value.ToString("X");
    }
}
