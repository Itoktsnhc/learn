#r "nuget: NBomber"
open NBomber
open System.Net.Http
open FSharp.Control.Tasks.NonAffine

open NBomber
open NBomber.Contracts
open NBomber.FSharp

use httpClient = new HttpClient()

let step = Step.create("fetch_step", fun context -> task {

    let! response = httpClient.GetAsync("http://localhost:8080/", context.CancellationToken)

    return if response.IsSuccessStatusCode then Response.ok(statusCode = int response.StatusCode)
           else Response.fail(statusCode = int response.StatusCode)
})

Scenario.create "simple_http" [step]
|> Scenario.withWarmUpDuration(seconds 10)
|> Scenario.withLoadSimulations [InjectPerSec(rate = 500, during = seconds 30)]
|> NBomberRunner.registerScenario
|> NBomberRunner.run
|> ignore