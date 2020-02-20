module TestLuhn

open Xunit
open FsUnit.Xunit
open SweIdNum.Core.Luhn

[<Fact>]
let ``1 pin works`` () =
    calculate_luhn "850504333" |> should equal 4
    calculate_luhn "121212121" |> should equal 2
    is_luhn_valid "8505043334" |> should equal true
    is_luhn_valid "1212121212" |> should equal true

