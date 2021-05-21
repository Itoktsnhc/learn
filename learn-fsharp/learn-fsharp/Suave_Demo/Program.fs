﻿// Learn more about F# at http://fsharp.org

open System
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful

let app =
    choose [ GET
             >=> choose [ path "/hello" >=> OK "Hello Get"
                          path "/goodbay" >=> OK "Good bye GET" ]
             POST
             >=> choose [ path "/hello" >=> OK "Hello POST"
                          path "/goodbye" >=> OK "Good bye POST" ] ]

startWebServer defaultConfig app
