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

[<MemoryDiagnoser>]
type Day04Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day04.solve 1

    [<Benchmark>]
    member this.Part2() = Day04.solve 2

[<MemoryDiagnoser>]
type Day05Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day05.solve 1

    [<Benchmark>]
    member this.Part2() = Day05.solve 2

[<MemoryDiagnoser>]
type Day06Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day06.solve 1

    [<Benchmark>]
    member this.Part2() = Day06.solve 2

[<MemoryDiagnoser>]
type Day07Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day07.solve 1

    [<Benchmark>]
    member this.Part2() = Day07.solve 2

[<MemoryDiagnoser>]
type Day08Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day08.solve 1

    [<Benchmark>]
    member this.Part2() = Day08.solve 2

[<MemoryDiagnoser>]
type Day09Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day09.solve 1

    [<Benchmark>]
    member this.Part2() = Day09.solve 2

[<MemoryDiagnoser>]
type Day10Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day10.solve 1

    [<Benchmark>]
    member this.Part2() = Day10.solve 2

[<MemoryDiagnoser>]
type Day11Benchmarks() =

    [<Benchmark>]
    member this.Part1() = Day11.solve 1

    [<Benchmark>]
    member this.Part2() = Day11.solve 2
