namespace AOC.Days

module Day10 =
    open AOC.Utils
    open CSharpLib

    type Lights =
        { Current: list<bool>
          Target: list<bool>
          Buttons: Set<int>[]
          Joltages: int[] }

    let parse_line (line: string) : Lights =
        let parts = line.Split(" ")

        let lights =
            parts.[0].[1 .. parts.[0].Length - 2]
            |> Seq.map (fun c -> if c = '.' then false else true)
            |> Seq.toList

        let buttons =
            parts.[1 .. parts.Length - 2]
            |> Array.map (fun btnStr ->
                btnStr.Replace("(", "").Replace(")", "").Split(',')
                |> Array.map int
                |> Set.ofArray)

        let joltages =
            parts.[parts.Length - 1].Replace("{", "").Replace("}", "").Split(',')
            |> Array.map int

        { Current = List.replicate (List.length lights) false
          Target = lights
          Buttons = buttons
          Joltages = joltages }

    let toogle_lights (lights: Lights) (button_idx: int) : Lights =
        let button = lights.Buttons.[button_idx]

        let new_current =
            lights.Current
            |> List.mapi (fun idx state -> if Set.contains idx button then not state else state)

        { lights with Current = new_current }

    let bfs (lights: Lights) : int =
        let rec loop (queue: list<(Lights * int)>) (visited: Set<list<bool>>) : int =
            match queue with
            | [] -> -1
            | (current_lights, depth) :: rest ->
                if current_lights.Current = current_lights.Target then
                    depth
                else
                    let next_states =
                        [ 0 .. current_lights.Buttons.Length - 1 ]
                        |> List.map (fun btn_idx -> toogle_lights current_lights btn_idx)
                        |> List.filter (fun l -> not (Set.contains l.Current visited))
                        |> List.map (fun l -> l, depth + 1)

                    let new_visited =
                        next_states |> List.fold (fun acc (l, _) -> Set.add l.Current acc) visited

                    loop (rest @ next_states) new_visited

        loop [ (lights, 0) ] (Set.empty.Add lights.Current)

    let make_matrices (buttons: Set<int>[]) (joltages: int[]) =
        let b = joltages |> Array.map double
        let c = Array.init buttons.Length (fun _ -> double 1.0)
        let A = Array2D.zeroCreate<double> joltages.Length buttons.Length

        [ 0 .. joltages.Length - 1 ]
        |> List.iter (fun i ->
            buttons
            |> Array.iteri (fun j btnSet ->
                if Set.contains i btnSet then
                    A.[i, j] <- 1.0))

        A, b, c


    let part1 (input: list<string>) : string =
        let lights = input |> List.map parse_line
        lights |> List.map bfs |> List.sum |> string

    let part2 (input: list<string>) : string =
        let lights = input |> List.map parse_line
        let tableous = lights |> List.map (fun l -> make_matrices l.Buttons l.Joltages)

        tableous
        |> List.map (fun (A, b, c) -> Helpers.MIPSolve(c, A, b))
        |> List.sum
        |> string

    let solve part =
        let data = readInputLines 10

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
