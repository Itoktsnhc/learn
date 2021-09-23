module Suave_Demo.Libs.Auth

open Newtonsoft.Json
open Suave

type LoginDetails = { Email: string; Password: string }
type Login = { User: LoginDetails }

let unauthorized s = Response.response HTTP_401 s
let UNAUTHORIZED s = unauthorized (UTF8.bytes s)

let validatePassword passwordHash passedInPassword =
    Hash.Crypto.verify passwordHash passedInPassword

let loginWithCredentials dbClient (ctx: HttpContext) =
    async {
        let deserializeToJson json =
            JsonConvert.DeserializeObject<Login>(json)

        let login =
            ctx.request.rawForm
            |> System.Text.Encoding.UTF8.GetString
            |> deserializeToJson

        0
    }
