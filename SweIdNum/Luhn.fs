module Luhn
open System
//https://en.wikipedia.org/wiki/Luhn_algorithm
let digits_of(number)=
     number.ToString() |> Seq.map(fun c->int(Char.GetNumericValue(c)))

let luhn_checksum(number:int64)=
    let digits = digits_of(number) |> Seq.mapi (fun i d-> (i,d) )
    let even i=i%2=0
    let odd_digits = digits |> Seq.filter (not<<even<<fst) |> Seq.map snd
    let even_digits = digits|> Seq.filter (even<<fst) |> Seq.map snd
    let mutable total = odd_digits |> Seq.sum
    for digit in even_digits do
        total <-total + (digits_of(2 * digit) |> Seq.sum)
    total % 10

let is_luhn_valid(number)=
    luhn_checksum(number) = 0

let calculate_luhn(partial_card_number:int64)=
    let check_digit = luhn_checksum(partial_card_number * 10L)      // Append a zero check digit to the partial number and calculate checksum
    if check_digit = 0 then check_digit                             // If the (sum mod 10) == 0, then the check digit is 0
    else 10 - check_digit                                           // Else, the check digit = 10 - (sum mod 10)