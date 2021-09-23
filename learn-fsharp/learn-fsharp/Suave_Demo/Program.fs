// Learn more about F# at http://fsharp.org
open System
open System.Text
open System.Text.Unicode
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.ServerErrors
open Suave.Writers
open Newtonsoft.Json

let setCORSHeaders =
    addHeader "Access-Control-Allow-Origin" "*"
    >=> addHeader "Access-Control-Allow-Headers" "content-type"

let allowCors: WebPart =
    choose [ OPTIONS
             >=> fun context ->
                     context
                     |> (setCORSHeaders >=> Successful.OK "CORS approved") ]


let serverConfig =
    let randomPort = Random().Next(7000, 7999)

    { defaultConfig with
          bindings = [ HttpBinding.createSimple HTTP "127.0.0.1" randomPort ] }
