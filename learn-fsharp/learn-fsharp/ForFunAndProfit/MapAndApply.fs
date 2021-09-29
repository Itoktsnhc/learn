module ForFunAndProfit.MapAndApply

module Option =
    // The apply function for Options
    let apply fOpt xOpt =
        match fOpt, xOpt with
        | Some f, Some x -> Some(f x) // 只有f 和 x 都存在
        | _ -> None

    let map f xOpt =
        match xOpt with
        | Some x -> f x |> Some
        | _ -> None

    let (<*>) = apply
    let (<!>) = map

    let lift2 f x y = f <!> x <*> y

    let lift2'' f x y =
        let mapped = map f x // (b->c) option
        apply mapped y // apply ((b->c) option) b => c option

    let lift3 f x y z = f <!> x <*> y <*> z

    let lift3' f x y z =
        let mapped = map f x //(b->c->d) option
        let first = apply mapped y // apply ((b->c->d) option) b
        apply first z

let addPair x y = x + y
let addPairOpt = Option.lift2 addPair

// define a three-parameter function to test with
let addTriple x y z = x + y + z

// lift a three-param function
let addTripleOpt = Option.lift3 addTriple

Option.lift2 (+) (Some 2) (Some 3) |> ignore
// define a tuple creation function
let tuple x y = x, y

// create a generic combiner of options
// with the tuple constructor baked in
let combineOpt x y = Option.lift2 tuple x y


// alternate "zip" implementation
// [f;g] apply [x;y] becomes [f x; g y]
let rec zipList fList xList =
    match fList, xList with
    | [], _
    | _, [] ->
        // either side empty, then done
        []
    | (f :: fTail), (x :: xTail) ->
        // new head + new tail
        (f x) :: (zipList fTail xTail)
// has type : ('a -> 'b) -> 'a list -> 'b list


//monadic function
let parseInt str =
    match str with
    | "-1" -> Some -1
    | "0" -> Some 0
    | "1" -> Some 1
    | "2" -> Some 2
    // etc
    | _ -> None

type OrderQty = OrderQty of int

let toOrderQty qty =
    if qty >= 1 then
        Some(OrderQty qty)
    else
        // only positive numbers allowed
        None

let (>>=) opt f = Option.bind f opt
let parseOrderQty str = parseInt str |> Option.bind toOrderQty


let parseOrderQty' str = str |> parseInt >>= toOrderQty
