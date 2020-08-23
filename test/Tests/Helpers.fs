module Helpers

let successFull parsed=
    match parsed with
    | Ok v-> v
    | Error e->failwithf "%s" (e.ToString())
let unSuccessFull parsed=
    match parsed with
    | Ok v-> failwithf "%A" v
    | Error e->e

let toString (pin)=pin.ToString()


