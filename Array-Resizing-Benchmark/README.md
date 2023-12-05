# Different ways Resize an Array

## Key Results

1. `Array.Resize` the **fastest** one with **zero allocation** (BUT it **just resize** the array and **do NOT create a new array**)
2. These have **similar performance and allocation**
   1. `ArraySegment_ToArray` (**NOT** `ArraySegment_CollectionExpression` of C# 12, because it's much slower)
   2. `Array.Copy`
   3. `Span.CopyTo`
   4. `Buffer.BlockCopy`
3. `Linq_Take_ToArray` is **the slowest** one with **most allocation**

![Benchmark](Benchmark.png)