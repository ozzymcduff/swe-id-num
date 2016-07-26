module TestControlPin
open Xunit
open FsUnit.Xunit
open SweIdNum
open SweIdNum.Core.PersonalIdentityNumbers
open Helpers

[<Fact>]
let control_number ()=
    isValid "196408233234" |> should equal true
    tryParse "196408233235" |> unSuccessFull |> should equal (InvalidChecksum (expected=4,actual=5))

