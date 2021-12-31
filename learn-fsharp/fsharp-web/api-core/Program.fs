namespace api_core

#nowarn "20"

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder args
        builder.Services.AddHttpClient()
        builder.Services.AddControllers()
        builder.Services.AddSwaggerGen()
        let app = builder.Build()
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers()
        app.Run()

        exitCode
