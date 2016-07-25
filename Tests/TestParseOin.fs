module TestParseOin
open Xunit
open FsUnit.Xunit
open SweIdNum
open SweIdNum.Core.OrganizationalIdentityNumbers
open Helpers

let toStringOfSuccessFullParse v : string=
    tryParse v |> successFull |> toString 

[<Fact>]
let ``character_GNNNNNN-NNNC`` ()=
    toStringOfSuccessFullParse "556000-4615" |> should equal "556000-4615"
    toStringOfSuccessFullParse "232100-0156" |> should equal "232100-0156"
    toStringOfSuccessFullParse "802002-4280" |> should equal "802002-4280"

[<Fact>]
let ``error expected`` ()=
    tryParse "802X024280" |> unSuccessFull |> should equal DoesNotMatchFormat
    tryParse "AA2002-4280" |> unSuccessFull |> should equal DoesNotMatchFormat
    tryParse "5560.0-4.15" |> unSuccessFull |> should equal DoesNotMatchFormat


