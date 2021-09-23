// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Saturn
open Giraffe

type Customer = { Name: string; Address: string }

let customers =
    choose [ GET
             >=> (json
                      { Name = "Mr. Smith"
                        Address = "Santa Monika" })
             PUT
             >=> (bindJson<Customer>
                      (fun customer ->
                          printfn $"Adding customer %A{customer}"
                          json customer)) ]

let webApp =
    choose [ route "/" >=> htmlFile "/pages/index.html"
             route "/api/customers" >=> customers ]

let app = application { use_router webApp }
run app
