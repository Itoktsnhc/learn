module ForFunAndProfit.UContinuations

let x = 42
let y = 23
let z = 89
//↑ equals to ↓
//let x = 42 in
//  let y = 43 in
//      let z = x + y in
//      z  the result

42
|> (fun x -> 43 |> (fun y -> x + y |> (fun z -> z)))


let pipeInto (exp, lambda) =
    printfn $"exp is {exp} into {lambda}"
    exp |> lambda

pipeInto (42, (fun x -> pipeInto (43, (fun y -> pipeInto (x + y, (fun z -> z))))))
