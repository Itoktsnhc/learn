module ForFunAndProfit.Intro


let log p = printfn $"expression is {p}"

let loggedWorkflow_basic =
    let x = 42
    log x
    let y = 43
    log y
    let z = x + y
    log z
    //return
    z

type LoggingBuilder() =
    let log p = printfn $"exp is {p}"

    member this.Bind(x, f) =
        log x
        f x

    member this.Return(x) = x

let logger = LoggingBuilder()

let loggedWorkflow () =
    logger {
        let! x = 42
        let! y = 43
        let! z = x + y
        return z
    }

let divideBy bottom top =
    if bottom = 0 then
        None
    else
        Some(top / bottom)

let divideByWorkflow_basic init x y z =
    let a = init |> divideBy x

    match a with
    | None -> None // give up
    | Some a' -> // keep going
        let b = a' |> divideBy y

        match b with
        | None -> None // give up
        | Some b' -> // keep going
            let c = b' |> divideBy z

            match c with
            | None -> None // give up
            | Some c' -> // keep going
                //return
                Some c'

type MayBeBuilder() =
    member this.Bind(x, f) =
        match x with
        | None -> None
        | Some v -> v |> f

    member _.Return(x) = Some x

let maybe = MayBeBuilder()

let divideByWorkflow i x y z =
    maybe {
        let! a = i |> divideBy x
        let! b = a |> divideBy y
        let! c = b |> divideBy z
        return c
    }

let map1 =
    [ ("1", "One"); ("2", "Two") ] |> Map.ofList

let map2 =
    [ ("A", "Alice"); ("B", "Bob") ] |> Map.ofList

let map3 =
    [ ("CA", "California")
      ("NY", "New York") ]
    |> Map.ofList

let multiLookup_basic key =
    match map1.TryFind key with
    | Some result1 -> Some result1 // success
    | None -> // failure
        match map2.TryFind key with
        | Some result2 -> Some result2 // success
        | None -> // failure
            match map3.TryFind key with
            | Some result3 -> Some result3 // success
            | None -> None // failure

multiLookup_basic "A" |> printfn "Result for A is %A"
multiLookup_basic "CA" |> printfn "Result for CA is %A"
multiLookup_basic "X" |> printfn "Result for X is %A"

type OrElseBuilder() =
    member this.ReturnFrom(x) = x

    member this.Combine(a, b) =
        match a with
        | Some _ -> a // a succeeds -- use it
        | None -> b // a fails -- use b instead

    member this.Delay(f) = f ()

let orElse = OrElseBuilder()

let multiLookup key = orElse {
    return! map1.TryFind key
    return! map2.TryFind key
    return! map3.TryFind key
    }
