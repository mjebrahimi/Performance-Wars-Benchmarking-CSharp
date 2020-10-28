# Ctor Instantiate vs Lazy Instantiate

IMO this is a micro-optimization but a **Considerable** one.

Lazy instantiation was about **120x faster** with **zero allocation**.

This can be very effective, especially when querying entities from the database.

![Benchmark](BenchmarkDotNet.Artifacts/Benchmark.png)