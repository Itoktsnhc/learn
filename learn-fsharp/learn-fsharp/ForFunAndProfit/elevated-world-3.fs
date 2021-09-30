module ForFunAndProfit.elevated_world_3

open System

type CustomerId = CustomerId of int
type EmailAddress = EmailAddress of string

type CustomerInfo =
    { id: CustomerId
      name: string
      email: EmailAddress }

type Result<'a> =
    | Success of 'a
    | Failure of string list

let createCustomerId id =
    if id > 0 then
        Success(CustomerId id)
    else
        Failure [ "CustomerId must be positive" ]

let createEmailAddress str =
    if System.String.IsNullOrEmpty(str) then
        Failure [ "Email must not be empty" ]
    elif str.Contains("@") then
        Success(EmailAddress str)
    else
        Failure [ "Email must contain @-sign" ]

module Result =
    let map f xResult =
        match xResult with
        | Success x -> Success(f x)
        | Failure errs -> Failure errs

    let return' x = Success x

    let apply fResult xResult =
        match fResult, xResult with
        | Success f, Success x -> Success(f x)
        | Failure errs, Success x -> Failure errs
        | Success f, Failure errs -> Failure errs
        | Failure errs1, Failure errs2 ->
            // concat both lists of errors
            Failure(List.concat [ errs1; errs2 ])

    let bind f xResult =
        match xResult with
        | Success x -> f x
        | Failure errs -> Failure errs

    type ResultBuilder() =
        member this.Return x = return' x
        member this.Bind(x, f) = bind f x


let (<!>) = Result.map
let (<*>) = Result.apply

let createCustomer customerId name email =
    { id = customerId
      name = name
      email = email }

let createCustomerResultA id name email =
    let idResult = createCustomerId id
    let emailResult = createEmailAddress email
    let nameResult = Success name

    createCustomer <!> idResult
    <*> nameResult
    <*> emailResult

let (>>=) x f = Result.bind f x
<*> (mapOption f tail)