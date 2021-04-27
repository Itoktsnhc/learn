type PriceConfig = { Percentage: double; Price: double }
let totalCap = 541502.0

let configs =
    [ { Percentage = 0.1; Price = 12.0 }
      { Percentage = 0.2; Price = 30.0 }
      { Percentage = 0.4; Price = 21.0 }
      { Percentage = 0.3; Price = 15.0 } ]

type ConfigResult =
    { Part: double
      Count: int
      Total: double }

let GenerateConfig () =
    let mutable carry = 0.0

    let res =
        configs
        |> List.map
            (fun x ->
                let mutable target =
                    (totalCap * x.Percentage) + (carry |> double)

                let p = x.Price * 100.0
                let mutable cnt = 0

                while target > 0.0 do
                    target <- target - p
                    cnt <- cnt + 1

                carry <- target + p

                { Part = p
                  Count = cnt - 1
                  Total = p * ((cnt - 1) |> double) })

    (res, carry)


let r, c = GenerateConfig()
r |> List.iter (fun x -> printfn "%O" x)

printfn "left:%O" c
