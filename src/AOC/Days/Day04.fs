namespace AOC.Days

module Day04 =
    open AOC.Utils

    let occupied_count (grid: Matrix<char>) r c =
        let neighbors = get_8_neighbors r c

        neighbors
        |> List.map (fun (nr, nc) -> grid.[nr, nc] = '@')
        |> List.filter id
        |> List.length

    let less_than_4 (grid: Matrix<char>) r c =
        let count = occupied_count grid r c
        let res = count < 4
        res

    let get_removable (grid: Matrix<char>) =
        [ 0 .. grid.Rows - 1 ]
        |> List.collect (fun r ->
            [ 0 .. grid.Cols - 1 ]
            |> List.filter (fun c -> grid.[r, c] = '@' && less_than_4 grid r c)
            |> List.map (fun c -> (r, c)))

    let remove to_remove (grid: Matrix<char>) =
        for (r, c) in to_remove do
            grid.[r, c] <- '.'


    let part1 input =
        let grid = Matrix.FromLines(input, id, '#')
        get_removable grid |> List.length |> string


    let part2 input =
        let grid = Matrix.FromLines(input, id, '#')

        let rec remove_all grid count =
            let to_remove = get_removable grid

            if List.isEmpty to_remove then
                count
            else
                remove to_remove grid
                remove_all grid (count + List.length to_remove)

        remove_all grid 0 |> string


    let solve part =
        let data = readInputLines 4

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
