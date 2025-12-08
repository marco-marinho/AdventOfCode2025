module Benchmarks

open BenchmarkDotNet.Attributes

open AOC.Days

[<MemoryDiagnoser>]
type Day01Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day01.solve 1

    [<Benchmark>]
    member this.Part2() = Day01.solve 2

[<MemoryDiagnoser>]
type Day02Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day02.solve 1

    [<Benchmark>]
    member this.Part2() = Day02.solve 2

[<MemoryDiagnoser>]
type Day03Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day03.solve 1

    [<Benchmark>]
    member this.Part2() = Day03.solve 2
