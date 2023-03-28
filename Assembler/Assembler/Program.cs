
using Assembler;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: Program.exe [input_file_path]");
            return;
        }

        var inputFile = args[0];
        var outputFile = Path.GetFileNameWithoutExtension(inputFile) + ".hex";
        var outputPath = Path.GetDirectoryName(inputFile);

        var code = File.ReadAllText(inputFile);

        var assembler = new ProgramAssembler();
        var assembledCode = assembler.Assemble(code);

        File.WriteAllText(outputPath + "\\" + outputFile, assembledCode);

        Console.WriteLine(code);

        Console.WriteLine($"Assembled code saved to {outputPath}/{outputFile}");
    }
}