module Benchmarks

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open AOC.Days
// MemoryDiagnoser is CRITICAL: It shows you how many bytes you allocated.
[<MemoryDiagnoser>]
type Day01Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day01.solve 1

    [<Benchmark>]
    member this.Part2() = Day01.solve 2

// MemoryDiagnoser is CRITICAL: It shows you how many bytes you allocated.
[<MemoryDiagnoser>]
type Day02Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day02.solve 1

    [<Benchmark>]
    member this.Part2() = Day02.solve 2
