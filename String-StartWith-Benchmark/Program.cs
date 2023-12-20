using BenchmarkDotNet.Running;

#if DEBUG

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("***** To achieve accurate results, set project configuration to Release mode. *****");

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("***** Waite 3 seconds for DEBUG MODE! *****");

Thread.Sleep(3000);

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfigDry()); //For Debugging
BenchmarkRunner.Run<Benchmark>(new DebugInProcessConfigDry());

#else

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args); //InProcessEmitToolchain or InProcessNoEmitToolchain
BenchmarkRunner.Run<Benchmark>();

#endif

Console.ReadLine();