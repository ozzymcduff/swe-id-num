module SweIdNum.Core.Luhn
open System
//https://en.wikipedia.org/wiki/Luhn_algorithm
let digitsOf(number)=
     number.ToString() |> Seq.map(fun c->int(Char.GetNumericValue(c)))

let luhnChecksum(number:string)=
    let digits = digitsOf(number) |> Seq.mapi (fun i d-> (i,d) )
    let even i=i%2=0
    let odd_digits = digits |> Seq.filter (not<<even<<fst) |> Seq.map snd
    let even_digits = digits|> Seq.filter (even<<fst) |> Seq.map snd
    let mutable total = odd_digits |> Seq.sum
    for digit in even_digits do
        total <-total + (digitsOf(2 * digit) |> Seq.sum)
    total % 10

let isLuhnValid(number)=
    luhnChecksum(number) = 0

let calculateLuhn(partialCardNumber:string)=
    let checkDigit = luhnChecksum(partialCardNumber + "0")      // Append a zero check digit to the partial number and calculate checksum
    if checkDigit = 0 then checkDigit                           // If the (sum mod 10) == 0, then the check digit is 0
    else 10 - checkDigit                                        // Else, the check digit = 10 - (sum mod 10)