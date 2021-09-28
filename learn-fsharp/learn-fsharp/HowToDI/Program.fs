// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

module Domain =
    type UserId = UserId of int
    type UserName = string
    type EmailAddress = EmailAddress of string

    type Profile =
        { UserId: UserId
          Name: UserName
          EmailAddress: EmailAddress }

    type EmailMessage = { To: EmailAddress; Body: string }

module Infrastructure =
    open Domain

    type ILogger =
        abstract Info : string -> unit
        abstract Error : string -> unit

    type InfrastructureError =
        | DbError of string
        | SmtpError of string

    type DbConnection = DbConnection of unit

    type IDbService =
        abstract NewDbConnection : unit -> DbConnection
        abstract QueryProfile : DbConnection -> UserId -> Async<Result<Profile, InfrastructureError>>
        abstract UpdateProfile : DbConnection -> Profile -> Async<Result<unit, InfrastructureError>>

    type SmtpCredentials = SmtpCredentials of unit

    type IEmailService =
        abstract SendChangNotification : SmtpCredentials -> EmailMessage -> Async<Result<unit, InfrastructureError>>

type ReaderM<'dependency, 'result> = 'dependency -> 'result

let run (d: 'dependency) (r: ReaderM<'dependency, 'result>) : 'result = r d

let map (f: 'a -> 'b) (r: ReaderM<'d, 'a>) : ReaderM<'d, 'b> = r >> f

let (<?>) = map

let apply (f: ReaderM<'d, 'a -> 'b>) (r: ReaderM<'d, 'a>) : ReaderM<'d, 'b> =
    fun dep ->
        let f' = run dep f
        let a = run dep r
        f' a

let (<*>) = apply

[<EntryPoint>]
let main argv = 0 // return an integer exit code
