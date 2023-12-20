using BenchmarkDotNet.Running;

#if DEBUG

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("***** To achieve accurate results, set project configuration to Release mode. *****");

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("***** Waite 3 seconds for DEBUG MODE! *****");

Thread.Sleep(3000);

BenchmarkRunner.Run<Benchmark2>(new DebugInProcessConfigDry());

#else

BenchmarkRunner.Run<Benchmark2>();

#endif

Console.ReadLine();