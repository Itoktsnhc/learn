module One

open Model
open System

let andThen f x =
    match x with
    | Some y -> y |> f
    | None -> None

type OptionBuilder() =
    member _.Bind(x, f) = andThen f x
    member _.Return(x) = Some x
    member _.ReturnFrom(x) = x


let chargeCard (amount: double) (card: CreditCard) : TransactionId option = None

let lookupUser (userId: UserId) : User option = None

let getCreditCard (user: User) : CreditCard option = user.CreditCard

let getLimit (user: User) : double option = user.Limit

let option = OptionBuilder()

let chargeUserCard (amount: double) (userId: UserId) =
    option {
        let! user = lookupUser userId
        let! card = getCreditCard user
        let! limit = getLimit user

        return!
            match amount <= limit with
            | true -> chargeCard amount card
            | false -> None
    }

let validateNumber number : Result<string, string> =
    if String.length number > 16 then
        Error "A credit card number must be less than 16 characters"
    else
        Ok number

let validateExpiry expiry : Result<string, string> = Error ""

let validateCvv cvv : Result<string, string> = Error ""

let createCreditCard number expiry cvv =
    { Number = number
      Expiry = expiry
      Cvv = cvv }

let apply a f =
    match f, a with
    | Ok g, Ok x -> g x |> Ok
    | Error e, Ok _ -> e |> Error
    | Ok _, Error e -> e |> Error
    | Error e1, Error _ -> e1 |> Error

let validateCreditCardS card : Result<CreditCard, string> =
    Ok(createCreditCard)
    |> apply (validateNumber card.Number)
    |> apply (validateExpiry card.Expiry)
    |> apply (validateCvv card.Cvv)

let applyOpt a f =
    match f, a with
    | Some g, Some x -> g x
    | _ -> None

let validateCreditCard (card: CreditCard) : Result<CreditCard, string list> = Error [ ""; "" ]
