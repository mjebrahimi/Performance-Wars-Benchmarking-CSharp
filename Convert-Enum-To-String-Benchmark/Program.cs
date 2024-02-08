using BenchmarkDotNet.Running;
using Convert_Enum_To_String_Benchmark;

class Program
{
    public static void Main(string[] args)
    {
#if DEBUG
        Console.ForegroundColor = System.ConsoleColor.Yellow;
        Console.WriteLine("*****To achieve accurate results, set project configuration to Release mode.*****");
        return;
#endif
        BenchmarkRunner.Run<Benchmark>();
    }
}