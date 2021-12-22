open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Web.Endpoints

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    let app = builder.Build()

    let logger:ILogger = app.Logger

    app.MapGet("/api/v1.0/get/{item}", Func<string,string>(fun item -> get item) ) |> ignore
    app.MapGet("/api/v1.0/add/{key}={value}", Func<string, string, string> (fun key value -> add (key, value, logger) ) ) |> ignore
    app.MapPost("/api/v1.0/add/", Func<_,_> (fun body -> addPost body logger)) |> ignore
    app.MapGet("/api/v1.0/del/{item}", Func<string,string>(fun item -> del item) ) |> ignore
    app.MapGet("/api/v1.0/purge", Func<string,string> purge ) |> ignore
    app.MapGet("/api/v1.0/contents", Func<string,string> contents ) |> ignore

    app.Run()

    0 // Exit code