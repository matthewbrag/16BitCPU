
using Assembler;

var assembler = new ProgramAssembler();

var code = @"

//input numbers
INP 0 r0
INP 1 r1

//subtract
INOP SUB r2, r0, r1

//jump if result is negative
JCN NEG NEG_OUTPUT

//if result is positive output to output 0
OUT 0 r2

//skip negative output
JMP END

LABEL NEG_OUTPUT

OUT 1 r2

LABEL END

HLT
";

Console.WriteLine(assembler.Assemble(code));