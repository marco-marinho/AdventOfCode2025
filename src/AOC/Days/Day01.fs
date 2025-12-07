namespace AOC.Days

module Day01 =
    open AOC.Utils

    let parse_line (line: string) = line[0], int line[1..]

    let rotate direction clicks current =
        let turns = clicks / 100
        let remaining_clicks = if 'L' = direction then -(clicks % 100) else clicks % 100
        let pre_mod = current + remaining_clicks

        let crossed =
            if (pre_mod > 100 || pre_mod < 0) && current <> 0 then
                1
            else
                0

        let end_point = (pre_mod % 100 + 100) % 100
        end_point, crossed + turns

    let get_crossing input =
        List.mapFold
            (fun pos (direction, clicks) ->
                let new_pos, crossed = rotate direction clicks pos
                (new_pos, crossed), new_pos)
            50
            input
        |> fst


    let part1 (input: (char * int) list) =
        input
        |> get_crossing
        |> List.sumBy (fun (p, _) -> if p = 0 then 1 else 0)
        |> string


    let part2 input =
        input
        |> get_crossing
        |> List.sumBy (fun (p, c) -> if p = 0 then c + 1 else c)
        |> string

    let solve part =
        let data = readInputLines 1 |> List.map parse_line

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
