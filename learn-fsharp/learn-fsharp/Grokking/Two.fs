module Grokking.Two

open Model

let apply a f =
    match f, a with
    | Ok g, Ok x -> g x |> Ok
    | Error e, Ok _ -> e |> Error
    | Ok _, Error e -> e |> Error
    | Error e1, Error e2 -> (e1 @ e2) |> Error
    
let validateCreditCard (card: CreditCard) : Result<CreditCard, string list> = Error [ ""; "" ]