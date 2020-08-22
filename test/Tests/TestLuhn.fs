module TestLuhn

open Xunit
open FsUnit.Xunit
open SweIdNum.Core.Luhn

[<Fact>]
let ``1 pin works`` () =
    calculateLuhn "850504333" |> should equal 4
    calculateLuhn "121212121" |> should equal 2
    isLuhnValid "8505043334" |> should equal true
    isLuhnValid "1212121212" |> should equal true

