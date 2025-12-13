namespace AOC.Days

module Day09 =
    open AOC.Utils
    open CSharpLib

    let compute_areas (points: Coordinate[]) =
        let output =
            Array.zeroCreate<DistanceResult> (points.Length * (points.Length - 1) / 2)

        Helpers.ComputeAreas(points, output)
        output

    let parse (lines: list<string>) =
        lines
        |> List.map (fun line ->
            line.Split(',')
            |> Array.map int
            |> fun arr -> Coordinate(X = arr.[1], Y = arr.[0]))
        |> List.toArray
    
    let compress (arr: int[]) =
        arr
        |> Array.distinct 
        |> Array.sort 
        |> Array.fold (fun acc x -> x+1 :: x :: acc) []
        |> List.skip 1
        |> List.rev
        |> List.distinct
        |> Array.ofList

    let get_compressed_index (coordinate:Coordinate) (uniqueX: int[]) (uniqueY: int[]) =
        let x_idx = System.Array.BinarySearch(uniqueX, coordinate.X)
        let y_idx = System.Array.BinarySearch(uniqueY, coordinate.Y)
        Coordinate(X = x_idx, Y = y_idx)
    
    let get_largest_valid (areas: DistanceResult[]) (points: Coordinate[]) (integral_grid: int[,]) (uniqueX: int[]) (uniqueY: int[]) =
        let rec check idx =
            let p1, p2, area = 
                areas.[idx].I, 
                areas.[idx].J, 
                areas.[idx].Area
            let compressed_p1 = get_compressed_index points.[p1] uniqueX uniqueY
            let compressed_p2 = get_compressed_index points.[p2] uniqueX uniqueY
            let x_start, x_end = min compressed_p1.X compressed_p2.X, max compressed_p1.X compressed_p2.X
            let y_start, y_end = min compressed_p1.Y compressed_p2.Y, max compressed_p1.Y compressed_p2.Y
            let target_area = (x_end - x_start) * (y_end - y_start)
            let lookup_sum = 
                integral_grid.[x_end, y_end] 
                - integral_grid.[x_start, y_end] 
                - integral_grid.[x_end, y_start] 
                + integral_grid.[x_start, y_start]
            if target_area = lookup_sum then
                area
            else
                check (idx + 1)  
        check 0

    let part1 (input: list<string>) : string =
        let points = parse input

        let output = compute_areas points
        output |> Array.map (fun r -> r.Area) |> Array.max |> string

    let part2 (input: list<string>) : string =
        let points = parse input
        let areas = compute_areas points |> Array.sortBy (fun r -> -r.Area)

        let unique_x = 
            points 
            |> Array.map (fun p -> p.X) 
            |> compress
        let unique_y = 
            points 
            |> Array.map (fun p -> p.Y) 
            |> compress
        let grid = Array2D.zeroCreate<int> unique_x.Length unique_y.Length
        let compresed_vectices = 
            points 
            |> Array.map (fun point -> get_compressed_index point unique_x unique_y)
            |> fun arr -> Array.append arr [| Array.head arr |]
            |> Array.windowed 2
        Helpers.FillPolygonGrid(compresed_vectices, grid)
        let flood_start = seq {0 .. Array2D.length1 grid - 1} |> Seq.find (fun i -> grid.[1, i] = 0 && grid.[0, i] = 1)
        Helpers.FloodFill(grid, flood_start)
        let integral_image = Helpers.IntegralImage grid 
        get_largest_valid areas points integral_image unique_x unique_y |> string


    let solve part =
        let data = readInputLines 9

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
