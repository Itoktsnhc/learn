namespace api_core.Controllers.HelloController

open System.Net.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<Route("api/hello")>]
[<ApiController>]
type HelloController(logger: ILogger<HelloController>, httpFactory: IHttpClientFactory) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Resp() =
        logger.LogError "This is a test!!!"

        task {
            logger.LogError "This is a test!!!"
            use client = httpFactory.CreateClient()
            let url = "https://www.baidu.com/"
            return! client.GetStringAsync(url)
        }
