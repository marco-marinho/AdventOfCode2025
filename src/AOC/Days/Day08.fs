namespace AOC.Days

module Day08 =
    open AOC.Utils
    open CSharpLib

    let helpers = Helpers()

    let parse (input: list<string>) : list<struct (uint64 * uint64 * uint64)> =
        input
        |> List.map (fun line ->
            let pieces = line.Split(",") |> Array.map uint64
            struct (pieces.[0], pieces.[1], pieces.[2]))

    let calc_distances (points: list<struct (uint64 * uint64 * uint64)>) =
        points
        |> List.mapi (fun i p1 -> i, p1) // Tag with index
        |> List.collect (fun (i, p1) ->
            // For every point, find distances to strictly higher indices
            points
            |> List.mapi (fun j p2 -> j, p2)
            |> List.filter (fun (j, _) -> j > i)
            |> List.map (fun (j, p2) -> (i, j), helpers.Distance3D(p1, p2)))

    let make_circuits distances max_connections =
        let (a, b), _ = List.head distances
        let first_circuit = [ set [ a; b ] ]

        let rec buildGraph edges (circuits: list<Set<int>>) connections =
            match edges with
            | [] -> circuits
            | ((x, y), _) :: tail ->
                if connections = max_connections then
                    circuits
                else

                    let circuits', added =
                        circuits
                        |> List.mapFold
                            (fun added circuit ->
                                if circuit.Contains x || circuit.Contains y then
                                    circuit |> Set.add x |> Set.add y, true
                                else
                                    circuit, added)
                            false

                    if not added then
                        buildGraph tail (set [ x; y ] :: circuits') (connections + 1)
                    else
                        buildGraph tail circuits' (connections + 1)

        buildGraph (List.tail distances) first_circuit 1

    let merge_circuits circuits =
        let rec merge acc remaining =
            match remaining with
            | [] -> acc
            | x :: xs ->
                let overlaps, rest =
                    xs |> List.partition (fun y -> not (Set.isEmpty (Set.intersect x y)))

                match overlaps with
                | [] -> merge (x :: acc) xs
                | _ ->
                    let new_circuit = overlaps |> List.fold (fun s c -> Set.union s c) x
                    merge acc (new_circuit :: rest)


        merge [] circuits

    let merge_all circuits remaining total =
        let rec loop current remaining =
            match remaining with
            | [] -> failwith "Could not merge all circuits"
            | ((x, y), _) :: xs ->
                let merged = merge_circuits (set [ x; y ] :: current)

                if List.length merged = 1 && Set.count merged.[0] = total then
                    x, y
                else
                    loop merged xs

        loop circuits remaining


    let part1 (input: list<string>) : string =
        let distances = parse input |> calc_distances |> List.sortBy (fun (_, dist) -> dist)
        let circuits = make_circuits distances 1000 |> merge_circuits

        let res =
            circuits
            |> List.sortBy (fun c -> -c.Count)
            |> List.take 3
            |> List.fold (fun acc c -> acc * c.Count) 1

        res |> string

    let part2 (input: list<string>) : string =
        let positions = parse input
        let distances = positions |> calc_distances |> List.sortBy (fun (_, dist) -> dist)
        let total = input.Length
        let circuits = make_circuits distances 1000 |> merge_circuits
        let remaining = distances |> List.skip 1000
        let a, b = merge_all circuits remaining total
        let struct (x1, _, _) = positions.[a]
        let struct (x2, _, _) = positions.[b]
        x1 * x2 |> string


    let solve part =
        let data = readInputLines 8

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
