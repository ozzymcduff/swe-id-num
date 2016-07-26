module TestControlOin
open Xunit
open FsUnit.Xunit
open SweIdNum
open SweIdNum.Core.OrganizationalIdentityNumbers
open Helpers

[<Fact>]
let ``character_oin_ctrl`` ()=
    isValid "556000-4615" |> should equal true
    isValid "232100-0156" |> should equal true
    isValid "802002-4280" |> should equal true
    tryParse "802002-4281" |> unSuccessFull |> should equal (InvalidChecksum (expected=0,actual=1))
