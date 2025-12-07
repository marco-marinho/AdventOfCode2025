namespace AOC.Days

module Day02 =
    open AOC.Utils

    let num_digits num =
        System.Math.Log10(float num) + 1.0 |> int

    let is_valid repeats id =
        let n_digits = num_digits id

        if n_digits % repeats <> 0 then
            false
        else
            let parts_pow = pown 10L (n_digits / repeats)
            let first_part = id % parts_pow

            let rec check_rest remaining iters =
                if iters = 0 then
                    true
                else
                    let current_part = remaining % parts_pow

                    if current_part <> first_part then
                        false
                    else
                        check_rest (remaining / parts_pow) (iters - 1)

            check_rest (id / parts_pow) (repeats - 1)

    let is_valid_multiple id =
        let n_digits = num_digits id

        seq { n_digits .. -1 .. 2 }
        |> Seq.exists (fun r -> n_digits % r = 0 && is_valid r id)


    let parseLine (lines: list<string>) =
        let parts = lines[0].Split(',')
        parts |> Array.map (fun x -> x.Split('-') |> Array.map int64)


    let part1 input =
        input
        |> parseLine
        |> Array.sumBy (fun range -> seq { range[0] .. range[1] } |> Seq.filter (is_valid 2) |> Seq.sum)
        |> string


    let part2 input =
        input
        |> parseLine
        |> Array.sumBy (fun range -> seq { range[0] .. range[1] } |> Seq.filter is_valid_multiple |> Seq.sum)
        |> string

    let solve part =
        let data = readInputLines 2

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
