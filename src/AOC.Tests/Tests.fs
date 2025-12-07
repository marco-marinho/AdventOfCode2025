module Day01Tests

open Xunit
open AOC.Days
open AOC.Utils

[<Fact>]
let ``Day 01 - Part 1`` () =
    let actual = Day01.solve 1
    Assert.Equal("1120", actual)

[<Fact>]
let ``Day 01 - Part 2`` () =
    let actual = Day01.solve 2
    Assert.Equal("6554", actual)

[<Fact>]
let ``Day 02 - Part 1`` () =
    let actual = Day02.solve 1
    Assert.Equal("16793817782", actual)

[<Fact>]
let ``Day 02 - Part 2`` () =
    let actual = Day02.solve 2
    Assert.Equal("27469417404", actual)
