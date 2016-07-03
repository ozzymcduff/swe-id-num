module TestLuhn

open Xunit
open FsUnit.Xunit
open Luhn

[<Fact>]
let ``1 pin works`` () =
    calculate_luhn 850504333L |> should equal 4
    calculate_luhn 121212121L |> should equal 2
    is_luhn_valid 8505043334L |> should equal true
    is_luhn_valid 1212121212L |> should equal true

