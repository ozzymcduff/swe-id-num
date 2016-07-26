module TestParse
open Xunit
open FsUnit.Xunit
open SweIdNum
open SweIdNum.Core.PersonalIdentityNumbers
open Helpers

[<Fact>]
let numeric_YYYYMMDDNNNC ()=
    parseNumeric 196408233234L |> successFull |> toString |> should equal "196408233234"
    parseNumeric 200108230004L |> successFull |> toString |> should equal "200108230004"

[<Fact>]
let numeric_YYMMDDNNNC ()=
    parseNumeric 6408233234L |> successFull |> toString |> should equal "196408233234"
    parseNumeric 108230004L |> successFull |> toString |> should equal "200108230004"
    parseNumeric 8230005L |> successFull |> toString |> should equal "200008230005"

[<Fact>]
let character_YYMMDDNNNC ()=
    tryParse "6408233234" |> successFull |> toString |> should equal "196408233234"
    tryParse "0008230005" |> successFull |> toString |> should equal "200008230005"

[<Fact>]
let character_YYYYMMDDNNNC ()=
    tryParse "196408233234" |> successFull |> toString |> should equal "196408233234"

[<Fact>]
let desc_different_formats ()=
    let pins = 
        ["19640823-3234";"196408233234"; "640823-3234"; "19640823-3234"; "6408233234"] 
        |> List.map tryParse
        |> List.map successFull
        |> List.map toString
        |> Set.ofList
    pins |> should equal (Set.ofList ["196408233234"])

[<Fact>]
let ``character_YYMMDD-NNNC`` ()=
    tryParse "640823-3234" |> successFull |> toString |> should equal "196408233234"
    tryParse "000823-0005" |> successFull |> toString |> should equal "200008230005"
    tryParse "000823+0005" |> successFull |> toString |> should equal "190008230005"

[<Fact>]
let ``error expected`` ()=
    tryParse "AA6408233234" |> unSuccessFull |> should equal DoesNotMatchFormat
    tryParse "196418233234" |> unSuccessFull |> should equal DoesNotMatchFormat

//[<Fact>]
let ``Recycling rules`` ()=
(*

test_pins <- c("18920822-2298", "18920822-2299", "19920419-1923")
test_that("Recycling rules", {
  expect_is(data.frame(as.pin(test_pins), 1:9), "data.frame")
  expect_equal(nrow(data.frame(as.pin(test_pins), 1:9)), 9)
  expect_equal(data.frame(as.pin(test_pins), 1:9)[1:3, 1], data.frame(as.pin(test_pins), 1:9)[4:6, 1])
  expect_equal(data.frame(as.pin(test_pins), 1:9)[1:3, 1], data.frame(as.pin(test_pins), 1:9)[7:9, 1])
}*)
    ""

[<Fact>]
let ``deceased 1947 - 1967`` ()=
    tryParse "550504333A" |> successFull |> toString |> should equal "19550504333A"
    tryParse "19280118123X" |> successFull |> toString |> should equal "19280118123X"
    tryParse "19280118123 " |> successFull |> toString |> should equal "19280118123 "

[<Fact>]
let ``deceased 1947 - 1967 born in the 18 hundreds`` ()=
    tryParse "850504111T" |> unSuccessFull |> should equal DoesNotMatchFormat
    tryParse "850504111 " |> unSuccessFull |> should equal DoesNotMatchFormat

    (*
  expect_message(as.pin(semi_pins[1]), "less than 100 years old and people with birth year")
  expect_message(as.pin(semi_pins[2]), "Assumption: People with birth year before 1967 and character")
  expect_message(as.pin(semi_pins[5]), "Assumption: People with birth year before 1967 and character")
  *)

