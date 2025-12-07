namespace AOC

module Utils =
    open System.IO
    let sourceDir = __SOURCE_DIRECTORY__

    let readInputLines (day: int) =
        let path = Path.Combine(sourceDir, "..", "..", "data", sprintf "day%02d.txt" day)

        File.ReadAllText(path).Split('\n', System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun s -> s.Trim())
        |> Array.toList
