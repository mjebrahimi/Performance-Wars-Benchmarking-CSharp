using BenchmarkDotNet.Running;

#region Class
//var classA1 = new ClassA() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };
//var classA2 = new ClassA() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };

//var classB1 = new ClassB() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };
//var classB2 = new ClassB() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };

//var a1 = classA1 == classA2;
//var a2 = classA1.Equals(classA2);
//var a3 = object.Equals(classA1, classA2);

//var b1 = classB1 == classB2;
//var b2 = classB1.Equals(classB2);
//var b3 = object.Equals(classB1, classB2);

//var x1 = classA1.GetHashCode();
//var x2 = classB1.GetHashCode();
#endregion

#region Struct
//var strucA1 = new StructA() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };
//var strucA2 = new StructA() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };

//var strucB1 = new StructB() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };
//var strucB2 = new StructB() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };

////var a1 = strucA1 == strucA2;
//var a2 = strucA1.Equals(strucA2);
//var a3 = object.Equals(strucA1, strucA2);

////var b1 = strucB1 == strucB2;
//var b2 = strucB1.Equals(strucB2);
//var b3 = object.Equals(strucB1, strucB2);

//var x1 = strucA1.GetHashCode();
//var x2 = strucB1.GetHashCode();
#endregion

#if DEBUG

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("***** To achieve accurate results, set project configuration to Release mode. *****");

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("***** Waite 3 seconds for DEBUG MODE! *****");

Thread.Sleep(3000);

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new BenchmarkDotNet.Configs.DebugInProcessConfig()); //For Debugging
BenchmarkRunner.Run<StructBenchmark>(new BenchmarkDotNet.Configs.DebugInProcessConfig());

#else

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
BenchmarkRunner.Run<StructBenchmark>();

#endif

Console.ReadLine();