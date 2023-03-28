using Assembler.Instructions;
using Assembler.Pre;

namespace Assembler;
internal class ProgramAssembler
{
    private readonly Preprocessor _preprocessor;
    private readonly Parser _parser;

    public ProgramAssembler()
    {

        _preprocessor = new Preprocessor();
        _parser = new Parser();


    }


    public string Assemble(string code)
    {
        var lines = _preprocessor.Preprocess(code);

        var instructions = lines.Select(line => _parser.GetInstruction(line)).ToArray();

        //first pass 
        ushort pc = 0;
        var factory = new AssemblerFactory();

        for(var i = 0; i < instructions.Length; i++)
        {
            var codes = factory.GetHex(instructions[i], pc);

            pc += (ushort)codes.Length;
        }

        //second pass
        var mappings = factory.GetMappings();

        var secondPassFactory = new AssemblerFactory(mappings);
        pc = 0;

        var hexCodes = new List<string>();

        for (var i = 0; i < instructions.Length; i++)
        {
            var codes = factory.GetHex(instructions[i], pc);

            pc += (ushort)codes.Length;

            hexCodes.AddRange(codes);
        }

        return string.Join("\n", hexCodes);
    }
}
