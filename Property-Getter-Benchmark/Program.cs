using BenchmarkDotNet.Running;

#if DEBUG
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("*****To achieve accurate results, set project configuration to Release mode.*****");
return;
#endif
BenchmarkRunner.Run<BenchmarkContainer>();
//new ManualConfig { Options = ConfigOptions.DisableLogFile };
//BenchmarkRunner.Run<BenchmarkContainer>(new DebugInProcessConfig());