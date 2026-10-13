# 16-Bit CPU

A 16-bit CPU designed from scratch in [Logisim](https://sourceforge.net/projects/circuit/), with its own instruction set, a hand-written C# assembler, and sample programs.

## What's here

- **`16_bit_CPU.circ`** — the full Logisim schematic. Separate integer and 32-bit floating-point ALUs, a 15-register file (each register wired to a fixed role rather than general-purpose), split 64K instruction/data RAM, clock and control logic, and I/O.
- **`16 bit CPU Instruction set.txt`** — the ISA: opcodes, register map, ALU operations, jump conditions, and the assembly mnemonics.
- **`Assembler/`** — a C# assembler that compiles the custom assembly language (labels, binary/hex/decimal/float literals, mnemonics like `LD`, `MOV`, `INOP`, `FLOP`, `JCN`) into `.hex` machine code for the program ROM.
- **`Programs/`** — sample assembly, including `AsciiNPrint` which walks a memory pointer and prints a null-terminated string.

## Architecture highlights

- Harvard-style split between 64K general-purpose RAM and 64K program RAM.
- Dedicated integer ALU (add, sub, multiply, rotate, not, and, or, xor) and float ALU (add, sub, multiply, divide, int↔float conversion).
- Conditional jumps on zero/positive/negative/overflow for both integer and float results.
- Fixed-purpose registers (ALU operands/result, I/O, RAM address/value pairs, program counter) instead of a general-purpose register file.

## Running it

1. Write assembly in the style of `Programs/AsciiNPrint.txt`.
2. Build and run the assembler: `Assembler.exe program.txt` → produces `program.hex`.
3. Open `16_bit_CPU.circ` in Logisim, load the `.hex` into program RAM, and run the simulation.

## Status

A working single-person hardware/software project, not actively maintained. Some instructions in the instruction-set doc (marked with a leading `-`) were planned but never implemented.
