namespace AOC.Days

module Day11 =
    open AOC.Utils
    open System.Collections.Generic

    let parse (input: list<string>) =
        let omap = Dictionary<string, string[]>()

        input
        |> List.iter (fun line ->
            let parts = line.Replace(":", "").Split(" ")
            let key = parts.[0]
            let values = parts.[1..]
            omap.[key] <- values)

        omap

    let count_connections (map: Dictionary<string, string[]>) (start: string) (target: string) : uint64 =
        let cache = Dictionary<string, uint64>()

        let rec loop (toVisit: string) =
            if toVisit = target then
                1UL
            else if cache.ContainsKey toVisit then
                cache.[toVisit]
            else
                let result =
                    match map.TryGetValue toVisit with
                    | false, _ -> 0UL
                    | true, nextVisits -> nextVisits |> Array.sumBy (fun nv -> loop nv)

                cache.[toVisit] <- result
                result

        loop start

    let part1 (input: list<string>) : string =
        let map = parse input
        count_connections map "you" "out" |> string

    let part2 (input: list<string>) : string =
        let map = parse input

        let fft_first =
            count_connections map "svr" "fft"
            * count_connections map "fft" "dac"
            * count_connections map "dac" "out"

        let dac_first =
            count_connections map "svr" "dac"
            * count_connections map "dac" "fft"
            * count_connections map "fft" "out"

        fft_first + dac_first |> string

    let solve part =
        let data = readInputLines 11

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
