namespace AOC.Days

module Day06 =
    open AOC.Utils

    type Operation =
        | Add
        | Multiply

    type Problem = { Op: Operation; Values: list<uint64> }

    let solve_problem problem =
        match problem.Op with
        | Add -> List.sum problem.Values
        | Multiply -> List.fold (*) 1UL problem.Values

    let parse_part_2 (input: list<string>) =
        let grid = Matrix.FromLines(input, id, ' ')

        [ 0 .. grid.Cols - 1 ]
        |> List.map (fun col ->
            let data = grid.Data.[*, col] |> Array.filter (fun c -> c <> ' ')
            data |> Array.fold (fun acc c -> acc * 10UL + uint64 (int c - int '0')) 0UL)

    let parse (input: list<string>) part =
        let rows = input.Length

        let values =
            if part = 1 then
                List.take (rows - 1) input
                |> List.map (fun line ->
                    line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
                    |> Array.map uint64
                    |> Array.toList)
                |> List.transpose
            else
                List.take (rows - 1) input |> parse_part_2 |> splitAt (fun x -> x = 0UL)

        let operations =
            input.[rows - 1].Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
            |> Array.map (fun opStr ->
                match opStr with
                | "+" -> Add
                | "*" -> Multiply
                | _ -> failwithf "Unknown operation: %s" opStr)
            |> Array.toList

        List.zip operations values
        |> List.map (fun (op, vals) -> { Op = op; Values = vals })


    let part1 (input: list<string>) =
        let problems = parse input 1
        problems |> List.map solve_problem |> List.sum |> string

    let part2 input =
        let problems = parse input 2
        problems |> List.map solve_problem |> List.sum |> string


    let solve part =
        let data = readInputLinesNoTrim 6

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
