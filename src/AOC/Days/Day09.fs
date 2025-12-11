namespace AOC.Days

module Day09 =
    open AOC.Utils
    open CSharpLib
    open System.Collections.Generic

    type Ranges =
        { Vertical: list<struct (int * int * int)>
          Horizontal: list<struct (int * int * int)> }

    let in_range (x: int) ((min, max): struct (int * int)) = x > min && x < max

    let compute_areas (points: Coordinate[]) =
        let output =
            Array.zeroCreate<DistanceResult> (points.Length * (points.Length - 1) / 2)

        Helpers.ComputeAreas(points, output)
        output

    let check_point_inside (point: Coordinate) (ranges: Ranges) =
        let in_horizontal () =
            ranges.Horizontal
            |> List.fold
                (fun acc (struct (edgeX, minY, maxY)) ->
                    if point.X = edgeX && point.Y >= minY && point.Y <= maxY then
                        "rato" |> inspect
                        true
                    else
                        acc)
                false

        let in_vectical () =
            ranges.Vertical
            |> List.fold
                (fun acc (struct (edgeY, minX, maxX)) ->
                    if point.Y = edgeY && point.X >= minX && point.X <= maxX then
                        "gato" |> inspect
                        true
                    else
                        acc)
                false

        let ray_cross () =
            let crossed =
                ranges.Vertical
                |> List.fold
                    (fun acc (struct (edgeY, minX, maxX)) ->
                        if point.Y < edgeY && in_range point.X struct (minX, maxX) then
                            (point.X, point.Y) |> inspect
                            (edgeY, minX, maxX) |> inspect
                            acc + 1
                        else
                            acc)
                    0

            crossed % 2 = 1

        in_horizontal () || in_vectical () || ray_cross ()

    let parse (lines: list<string>) =
        lines
        |> List.map (fun line ->
            line.Split(',')
            |> Array.map int
            |> fun arr -> Coordinate(X = arr.[1], Y = arr.[0]))
        |> List.toArray

    let get_largest_valid (points: Coordinate[]) (areas: DistanceResult[]) (ranges: Ranges) =

        let rec inner index =
            let i, j, area = areas.[index].I, areas.[index].J, areas.[index].Area
            let p1 = points.[i]
            let p2 = points.[j]
            let p3 = Coordinate(X = p1.X, Y = p2.Y)
            let p4 = Coordinate(X = p2.X, Y = p1.Y)

            if check_point_inside p3 ranges && check_point_inside p4 ranges then
                area
            else if index + 1 < areas.Length then
                inner (index + 1)
            else
                0L

        inner 0


    let part1 (input: list<string>) : string =
        let points = parse input

        let output = compute_areas points
        output |> Array.map (fun r -> r.Area) |> Array.max |> string

    let part2 (input: list<string>) : string =
        let points = parse input

        let put_in_ranges (coord1: Coordinate) (coord2: Coordinate) (range: Ranges) =
            if coord1.X = coord2.X then
                let minY = min coord1.Y coord2.Y
                let maxY = max coord1.Y coord2.Y

                { range with
                    Horizontal = struct (coord1.X, minY, maxY) :: range.Horizontal }
            else if coord1.Y = coord2.Y then
                let minX = min coord1.X coord2.X
                let maxX = max coord1.X coord2.X

                { range with
                    Vertical = struct (coord1.Y, minX, maxX) :: range.Vertical }
            else
                range

        let first =
            put_in_ranges points.[points.Length - 1] points.[0] { Vertical = []; Horizontal = [] }

        let ranges =
            points
            |> Array.windowed 2
            |> Array.fold (fun acc window -> put_in_ranges window.[0] window.[1] acc) first

        let areas = compute_areas points |> Array.sortBy (fun r -> -r.Area)

        get_largest_valid points areas ranges |> string


    let solve part =
        let data = readInputLines 9

        match part with
        | 1 -> part1 data
        | 2 -> part2 data
        | _ -> failwith "Invalid part number"
