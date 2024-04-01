# MediatR vs Mediator vs SlimMediator Benchmark

**Key Results:**
- [MediatR](https://github.com/jbogard/MediatR) and [Mediator](https://github.com/martinothamar/Mediator) (source generated) perform **Similar** and there is no big difference between them.
- [SlimMediator](https://github.com/mjebrahimi/Performance-Wars-Benchmarking-CSharp/tree/master/Mediators-Benchmark/SlimMediator) is **3~4x Faster** than MediatR and Mediator with about **10x less Allocation**. (usage: [here](https://github.com/mjebrahimi/Performance-Wars-Benchmarking-CSharp/blob/master/Mediators-Benchmark/Application/Mediator.UseCase.cs) and [here](https://github.com/mjebrahimi/Performance-Wars-Benchmarking-CSharp/blob/master/Mediators-Benchmark/Program.cs) - it can be a drop-in replacement for MediatR)

![Benchmark](Benchmark.png)