module HowToDI.Samp

open System.Threading.Tasks

[<Interface>]
type ILogger =
    abstract Debug : string -> unit
    abstract Error : string -> unit

[<Interface>]
type ILog =
    abstract Logger : ILogger


module Log =

    let debug (env: #ILog) fmt = Printf.kprintf env.Logger.Debug fmt
    let error (env: #ILog) fmt = Printf.kprintf env.Logger.Error fmt

[<Interface>]
type IDatabase =
    abstract Query : string * 'i -> Task<'o>
    abstract Execute : string * 'i -> Task


[<Interface>]
type IDb =
    abstract Database : IDatabase

module Db =
    let fetchUser (env: #IDb) userId =
        env.Database.Query("", {| userId = userId |})

    let updateUser (env: #IDb) user =
        env.Database.Execute("UpdateUser", user)

[<Struct>]
type Effect<'env, 'out> = Effect of ('env -> 'out)

module Effect =
    /// Create value with no dependency requirements.
    let inline value (x: 'out) : Effect<'env, 'out> = Effect(fun _ -> x)
    /// Create value which uses depenendency.
    let inline apply (fn: 'env -> 'out) : Effect<'env, 'out> = Effect fn

let foo env =
    let user = Db.fetchUser env 123
    Log.debug env $"User: %A{user}"
