namespace AOC.Days

module Day05 =
    open AOC.Utils

    let merge_ranges ((start1, end1): uint64 * uint64) ((start2, end2): uint64 * uint64) =
        if end1 < start2 || end2 < start1 then
            None
        else
            Some(min start1 start2, max end1 end2)

    let merge_all_ranges (ranges: list<uint64 * uint64>) =
        let sorted_ranges = List.sortBy fst ranges

        let rec merge_helper remaining merged =
            match remaining with
            | [] -> List.rev merged
            | x :: xs ->
                match merged with
                | [] -> merge_helper xs [ x ]
                | last :: rest ->
                    match merge_ranges last x with
                    | Some merged_range -> merge_helper xs (merged_range :: rest)
                    | None -> merge_helper xs (x :: merged)

        merge_helper sorted_ranges []

    let in_range (value: uint64) ((start_, end_): uint64 * uint64) = value >= start_ && value <= end_

    let length_of_range ((start_, end_): uint64 * uint64) = end_ - start_ + 1UL


    let parse_input input =
        let (ranges, ingredients) =
            List.partition (fun (line: string) -> line.Contains "-") input

        let parsed_ranges =
            ranges
            |> List.map (fun line ->
                let parts = line.Split [| '-' |]
                (uint64 parts.[0], uint64 parts.[1]))

        (merge_all_ranges parsed_ranges, ingredients |> List.map (fun s -> int64 s))

    let part1 (input: list<string>) =
        let (ranges, ingredients) = parse_input input

        ingredients
        |> List.filter (fun x -> List.exists (in_range (uint64 x)) ranges)
        |> List.length
        |> string

    let part2 input =
        let (ranges, ingredients) = parse_input input

        ranges |> List.map length_of_range |> List.sum |> string


    let solve part =
        let data = readInputLines 5

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
