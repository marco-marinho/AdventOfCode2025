namespace AOC

module Utils =
    open System.IO

    let sourceDir = __SOURCE_DIRECTORY__

    let inspect x =
        printfn "%A" x
        x

    type Matrix<'T>(data: 'T[,], defaultValue: 'T) =
        member this.Rows = Array2D.length1 data
        member this.Cols = Array2D.length2 data
        member this.Data = data

        member this.Item
            with get (row: int, col: int) =
                if row < 0 || row >= this.Rows || col < 0 || col >= this.Cols then
                    defaultValue
                else
                    data[row, col]
            and set (row: int, col: int) (value: 'T) =
                if row >= 0 && row < this.Rows && col >= 0 && col < this.Cols then
                    data[row, col] <- value

        static member FromLines(lines: list<string>, parseChar: char -> 'T, defaultVal: 'T) =
            let rows = lines.Length
            let cols = lines.[0].Length
            let arr = Array2D.init rows cols (fun r c -> parseChar lines.[r].[c])
            Matrix(arr, defaultVal)

    let readInputLines (day: int) =
        let path = Path.Combine(sourceDir, "..", "..", "data", sprintf "day%02d.txt" day)

        File.ReadAllText(path).Split('\n', System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun s -> s.Trim())
        |> Array.toList

    let readInputLinesNoTrim (day: int) =
        let path = Path.Combine(sourceDir, "..", "..", "data", sprintf "day%02d.txt" day)

        File.ReadAllText(path).Split('\n', System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun s -> s.Replace("\r", "")) |> Array.toList

    let get_8_neighbors (row: int) (col: int) =
        [ (row - 1, col - 1)
          (row - 1, col)
          (row - 1, col + 1)
          (row, col - 1)
          (row, col + 1)
          (row + 1, col - 1)
          (row + 1, col)
          (row + 1, col + 1) ]
    let splitAt predicate lst =
        let rec helper acc current = function
            | [] -> if List.isEmpty current then acc else acc @ [List.rev current]
            | x :: xs ->
                if predicate x then
                    let newAcc = if List.isEmpty current then acc else acc @ [List.rev current]
                    helper newAcc [] xs
                else
                    helper acc (x :: current) xs
        helper [] [] lst
