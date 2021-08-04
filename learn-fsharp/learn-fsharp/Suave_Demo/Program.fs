// Learn more about F# at http://fsharp.org

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful

let browseHome: WebPart =
    path "/hello"
    >=> choose [ RequestErrors.NOT_FOUND "Found no handlers" ]

let requiresAuthentication _ =
    choose [ GET >=> path "/public" >=> OK "Default GET"
             // Access to handlers after this one will require authentication
             Authentication.authenticateBasic
                 (fun (user, pwd) -> user = "foo" && pwd = "bar")
                 (choose [ GET
                           >=> path "/whereami"
                           >=> OK(sprintf "Hello authenticated person ")
                           GET >=> path "/" >=> OK("Some path")
                           GET >=> browseHome ]) ] // Serves file if exists

let app = requiresAuthentication ""

startWebServer defaultConfig app
