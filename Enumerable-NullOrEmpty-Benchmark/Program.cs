using BenchmarkDotNet.Running;

//var benchmark = new Benchmark();

////Check the functionality of the methods below
//benchmark.Array_Empty_ConditionalIfAny().ShouldBe(true);
//benchmark.Array_Empty_ConditionalIfLength().ShouldBe(true);
//benchmark.Array_Empty_IfNullOrAny().ShouldBe(true);
//benchmark.Array_Empty_IfNullOrLength().ShouldBe(true);
//benchmark.Array_Empty_PatternMatching().ShouldBe(true);
//benchmark.Array_Empty_TryGetNonEnumeratedCount().ShouldBe(true);

//benchmark.Array_NotEmpty_ConditionalIfAny().ShouldBe(false);
//benchmark.Array_NotEmpty_ConditionalIfLength().ShouldBe(false);
//benchmark.Array_NotEmpty_IfNullOrAny().ShouldBe(false);
//benchmark.Array_NotEmpty_IfNullOrLength().ShouldBe(false);
//benchmark.Array_NotEmpty_PatternMatching().ShouldBe(false);
//benchmark.Array_NotEmpty_TryGetNonEnumeratedCount().ShouldBe(false);

//benchmark.Array_Null_ConditionalIfAny().ShouldBe(true);
//benchmark.Array_Null_ConditionalIfLength().ShouldBe(true);
//benchmark.Array_Null_IfNullOrAny().ShouldBe(true);
//benchmark.Array_Null_IfNullOrLength().ShouldBe(true);
//benchmark.Array_Null_PatternMatching().ShouldBe(true);
//benchmark.Array_Null_TryGetNonEnumeratedCount().ShouldBe(true);

//benchmark.List_Empty_ConditionalIfAny().ShouldBe(true);
//benchmark.List_Empty_ConditionalIfLength().ShouldBe(true);
//benchmark.List_Empty_IfNullOrAny().ShouldBe(true);
//benchmark.List_Empty_IfNullOrLength().ShouldBe(true);
//benchmark.List_Empty_PatternMatching().ShouldBe(true);
//benchmark.List_Empty_TryGetNonEnumeratedCount().ShouldBe(true);

//benchmark.List_NotEmpty_ConditionalIfAny().ShouldBe(false);
//benchmark.List_NotEmpty_ConditionalIfLength().ShouldBe(false);
//benchmark.List_NotEmpty_IfNullOrAny().ShouldBe(false);
//benchmark.List_NotEmpty_IfNullOrLength().ShouldBe(false);
//benchmark.List_NotEmpty_PatternMatching().ShouldBe(false);
//benchmark.List_NotEmpty_TryGetNonEnumeratedCount().ShouldBe(false);

//benchmark.List_Null_ConditionalIfAny().ShouldBe(true);
//benchmark.List_Null_ConditionalIfLength().ShouldBe(true);
//benchmark.List_Null_IfNullOrAny().ShouldBe(true);
//benchmark.List_Null_IfNullOrLength().ShouldBe(true);
//benchmark.List_Null_PatternMatching().ShouldBe(true);
//benchmark.List_Null_TryGetNonEnumeratedCount().ShouldBe(true);

//Console.ForegroundColor = ConsoleColor.Green;
//Console.WriteLine("***** Functionality is Correct *****");

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