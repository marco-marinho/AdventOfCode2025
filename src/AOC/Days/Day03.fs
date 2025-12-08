namespace AOC.Days

module Day03 =
    open AOC.Utils
    open System.Collections.Generic

    let parse_line (line: string) =
        line |> Seq.map (fun c -> uint64 c - uint64 '0') |> Seq.toList

    let dynamic_program (input: list<uint64>) selectable =
        let memo = Dictionary<(int * int), uint64>()

        let rec input_helper (left: list<uint64>) to_pick index =
            if to_pick = 0 || to_pick > List.length left then
                0UL
            else
                let key = index, to_pick

                if memo.ContainsKey key then
                    memo.[key]
                else
                    let x = List.head left
                    let xs = List.tail left
                    let with_x = pown 10UL (to_pick - 1) * x + input_helper xs (to_pick - 1) (index + 1)
                    let without_x = input_helper xs to_pick (index + 1)
                    let result = max with_x without_x
                    memo.[key] <- result
                    result

        input_helper input selectable 0

    let part1 input =
        let banks = List.map parse_line input
        List.sumBy (fun x -> dynamic_program x 2) banks |> string


    let part2 input =
        let banks = List.map parse_line input
        List.sumBy (fun x -> dynamic_program x 12) banks |> string

    let solve part =
        let data = readInputLines 3

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
