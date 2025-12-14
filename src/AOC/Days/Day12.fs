namespace AOC.Days

module Day12 =
    open AOC.Utils
    open System.Collections.Generic

    let count_on_block (block: list<string>) : int =
        block
        |> List.sumBy (fun line -> line |> Seq.filter (fun c -> c = '#') |> Seq.length)

    let parse_region_line (line: string) =
        line.Replace(":", "").Replace("x", " ").Split(" ") |> Array.map int

    let parse (input: list<string>) =
        let shape_part = List.take 24 input
        let regions_part = List.skip 24 input

        let shape_sizes =
            List.chunkBySize 4 shape_part |> List.map count_on_block |> List.toArray

        let regions = regions_part |> List.map parse_region_line
        shape_sizes, regions

    let check_region_valid (shape_sizes: int[]) (region: int[]) =
        let num_blocks = region.[2..] |> Array.sum
        let area = region.[0] * region.[1]

        if area / 9 >= num_blocks then
            1
        else
            let required_area =
                Array.zip shape_sizes region.[2..]
                |> Array.sumBy (fun (size, count) -> size * count)

            if required_area <= area then 1 else 0


    let part1 (input: list<string>) : string =
        let shape_sizes, regions = parse input
        regions |> List.sumBy (check_region_valid shape_sizes) |> string

    let solve part =
        let data = readInputLines 12

        match part with
        | 1 -> part1 data
        | 2 -> "No part 2"
        | _ -> failwith "Invalid part number"
