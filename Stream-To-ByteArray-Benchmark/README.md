# Different ways to read byte[] from Stream

## Key Results

1. Both `ReadAllBytes` and `UsingBinaryReader` are **the best** and have **similar performance and allocation**
2. `ReadAllBytes` **does support Async** by `ReadAllBytesAsync` (but `UsingBinaryReader` **does NOT**)
3. Both `FileOptions.Sequential` and `FileOptions.None` for `FileStream` have **similar performance and are faster than** `FileOptions.Asynchronous`
4. Both `UsingMemoryStream` and `UsingRecyclableMemoryStream` usually have **similar performance** speed but the first one has **2x more allocation**
5. Using `UsingRecyclableMemoryStream` is better than `UsingMemoryStream` specially for **large** streams
6. `Sync` methods are **faster** than `Async` methods
7. `Async` methods with `FileOptions.None | Sequential` are **faster** than `FileOptions.Asynchronous`

**Sort by Performance (speed + allocation)**
2. ReadAllBytes - UsingBinaryReader[NoAsync]
3. UsingRecyclableMemoryStream
4. UsingMemoryStream

![Benchmark](Benchmark.png)
