``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1415 (20H2/October2020Update)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4420.0), X86 LegacyJIT
  DefaultJob : .NET Framework 4.8 (4.8.4420.0), X86 LegacyJIT


```
|              Method |         Mean |      Error |       StdDev |       Median |
|-------------------- |-------------:|-----------:|-------------:|-------------:|
| SearchInStringArray | 25,971.00 ns | 517.977 ns | 1,327.772 ns | 25,514.86 ns |
|        SearchInHash |     39.96 ns |   0.816 ns |     0.972 ns |     39.57 ns |
