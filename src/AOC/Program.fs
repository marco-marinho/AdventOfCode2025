module Program

open AOC.Days
open Benchmarks
open BenchmarkDotNet.Running

[<EntryPoint>]
let main args =
    match args with
    | [| "benchmark"; dayStr |] ->
        let day = int dayStr

        match day with
        | 1 ->
            let summary = BenchmarkRunner.Run<Day01Benchmarks>()
            ()
        | 2 ->
            let summary = BenchmarkRunner.Run<Day02Benchmarks>()
            ()
        | 3 ->
            let summary = BenchmarkRunner.Run<Day03Benchmarks>()
            ()
        | 4 ->
            let summary = BenchmarkRunner.Run<Day04Benchmarks>()
            ()
        | _ -> printfn "Day %d not implemented yet" day

    | [| dayStr; partStr |] ->
        let day = int dayStr
        let part = int partStr

        match day with
        | 1 ->
            let result = Day01.solve part
            printfn "Day %d, Part %d: %A" day part result
        | 2 ->
            let result = Day02.solve part
            printfn "Day %d, Part %d: %A" day part result
        | 3 ->
            let result = Day03.solve part
            printfn "Day %d, Part %d: %A" day part result
        | 4 ->
            let result = Day04.solve part
            printfn "Day %d, Part %d: %A" day part result
        | 5 ->
            let result = Day05.solve part
            printfn "Day %d, Part %d: %A" day part result
        | _ -> printfn "Day %d not implemented yet" day


    | _ ->
        printfn "Usage: dotnet run -- <day> [part]"
        printfn "Example: dotnet run -- 1 2"

    0
