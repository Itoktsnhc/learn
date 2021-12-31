namespace api_core

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder args
        builder.Services.AddHttpClient()
        builder.Services.AddControllers()
        builder.Services.AddSwaggerGen()


        let app = builder.Build()
        app.UseSwagger()
        app.UseSwaggerUI()

        app.MapControllers()
        app.Run()

        exitCode
