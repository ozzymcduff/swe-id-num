module TestFormatPin

open Xunit
open FsUnit.Xunit
open SweIdNum
open SweIdNum.Core
open PersonalIdentityNumbers
[<Fact>]
let FormatPin () =
    let maybePin = tryParse("191212121212")
    match maybePin with 
    | Choice1Of2 pin->
        pin |> format "NNNC" |> should equal "1212"
        pin |> format "P" |> should equal "(19) 12-12-12 - 1212"
        pin |> format "yyyy-mm-dd-NNNC" |> should equal "1912-12-12-1212"
    | Choice2Of2 e->
        failwithf "%A" e