module TestParseOin
open Xunit
open FsUnit.Xunit
open SweIdNum
open SweIdNum.PersonalIdentityNumbers
open Helpers

[<Fact>]
let ``character_GNNNNNN-NNNC`` ()=
    parse "556000-4615" |> successFull |> toString |> should equal "556000-4615"
    parse "232100-0156" |> successFull |> toString |> should equal "232100-0156"
    parse "802002-4280" |> successFull |> toString |> should equal "802002-4280"

[<Fact>]
let ``error expected`` ()=
    parse "802X024280" |> unSuccessFull |> should equal DoesNotMatchFormat
    parse "AA2002-4280" |> unSuccessFull |> should equal DoesNotMatchFormat
    parse "5560.0-4.15" |> unSuccessFull |> should equal DoesNotMatchFormat


