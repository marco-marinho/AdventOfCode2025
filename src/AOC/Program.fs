module Program

open AOC.Days

[<EntryPoint>]
let main args =
    match args with
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
        | _ -> printfn "Day %d not implemented yet" day

    | _ ->
        printfn "Usage: dotnet run -- <day> [part]"
        printfn "Example: dotnet run -- 1 2"

    0
