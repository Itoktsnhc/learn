#r "nuget: Suave"

open System
open System.Threading
open Suave

let cts = new CancellationTokenSource()
let conf = { defaultConfig with cancellationToken = cts.Token }
let listening, server = startWebServerAsync conf (Successful.OK "Hello World")

Async.Start(server, cts.Token)
printfn "Make requests now"
Console.ReadKey true |> ignore

cts.Cancel()