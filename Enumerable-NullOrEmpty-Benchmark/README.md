# Different ways to Check NotNullOrEmpty for collections

## Key Results
1. `IfNullOrLength`, `ContitionalIfLength`, and `PatternMatching` have **Similar** performance.
2. Prefer to check by `Length/Count` wherever possible instead of using `Any()` method.
3. Both these methods uses Linq Any **BUT** `collection?.Any() != true` is **Slower** than `collection is null || !collection.Any()` for **not-null** collections
4. For **General (IEnumerable) purpose**, using `TryGetNonEnumerableCount()` is **Faster** than Linq `Any()` method.
5. Using `TryGetNonEnumerableCount()` for **Array** is **Slower than** for **List** (not-null). (***so prefer to use Array PatternMatching at first then check `TryGetNonEnumerableCount()`***) (**👉 See Benchmark2**)

![Benchmark](Benchmark.png)

**Benchmark2**
![Benchmark2.png](Benchmark2.png)