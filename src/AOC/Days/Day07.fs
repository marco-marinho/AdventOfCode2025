namespace AOC.Days

module Day07 =
    open AOC.Utils
    open System.Collections.Generic

    let process_row (row: array<char>) (active: Set<int>) : Set<int> * int =
        active
        |> Set.fold
            (fun (acc, splits) active_index ->
                if row.[active_index] = '^' then
                    acc |> Set.add (active_index - 1) |> Set.add (active_index + 1), splits + 1
                else
                    acc |> Set.add active_index, splits)
            (set [], 0)

    let dfs (pos: (int * int)) (grid: Matrix<char>) : uint64 =
        let memo = Dictionary<(int * int), uint64>()

        let rec dfs' (pos': (int * int)) =
            let row, col = pos'

            if row = grid.Rows - 1 then
                1UL
            else
                match memo.TryGetValue pos' with
                | true, value -> value
                | false, _ ->
                    let result =
                        match grid.[row, col] with
                        | '^' -> dfs' (row + 1, col - 1) + dfs' (row + 1, col + 1)
                        | _ -> dfs' (row + 1, col)

                    memo.[pos'] <- result
                    result

        dfs' pos

    let part1 (input: list<string>) : string =
        let grid = Matrix.FromLines(input, id, '#')
        let start_col = Array.findIndex (fun s -> s = 'S') grid.Data.[0, *]
        let current_active = set [ start_col ]

        let _, total_splits =
            [ 1 .. grid.Rows - 1 ]
            |> List.fold
                (fun (active, splits) row_index ->
                    let new_active, new_splits = process_row grid.Data.[row_index, *] active
                    new_active, splits + new_splits)
                (current_active, 0)

        total_splits |> string

    let part2 (input: list<string>) : string =
        let grid = Matrix.FromLines(input, id, '#')
        let start_col = Array.findIndex (fun s -> s = 'S') grid.Data.[0, *]
        let total_paths = dfs (0, start_col) grid
        total_paths |> string

    let solve part =
        let data = readInputLines 7

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
