using BenchmarkDotNet.Running;

//FileEncodingDetector.DetectEncoding.Detect();

//var tempDirectory = Path.Combine(Path.GetTempPath(), "benchmark_temp");
//Directory.CreateDirectory(tempDirectory);

#if DEBUG

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("***** To achieve accurate results, set project configuration to Release mode. *****");

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("***** Waite 3 seconds for DEBUG MODE! *****");

Thread.Sleep(3000);

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new BenchmarkDotNet.Configs.DebugInProcessConfig()); //For Debugging
BenchmarkRunner.Run<Benchmark>(new BenchmarkDotNet.Configs.DebugInProcessConfig());

#else

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
BenchmarkRunner.Run<Benchmark>();

#endif

//Cleanup temp files
//Directory.Delete(tempDirectory, true);

Console.ReadLine();