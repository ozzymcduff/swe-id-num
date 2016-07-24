module TestParseOin
open Xunit
open FsUnit.Xunit
open SweIdNum
open SweIdNum.Core.OrganizationalIdentityNumbers
open Helpers

[<Fact>]
let ``character_GNNNNNN-NNNC`` ()=
    tryParse "556000-4615" |> successFull |> toString |> should equal "556000-4615"
    tryParse "232100-0156" |> successFull |> toString |> should equal "232100-0156"
    tryParse "802002-4280" |> successFull |> toString |> should equal "802002-4280"

[<Fact>]
let ``error expected`` ()=
    tryParse "802X024280" |> unSuccessFull |> should equal DoesNotMatchFormat
    tryParse "AA2002-4280" |> unSuccessFull |> should equal DoesNotMatchFormat
    tryParse "5560.0-4.15" |> unSuccessFull |> should equal DoesNotMatchFormat


