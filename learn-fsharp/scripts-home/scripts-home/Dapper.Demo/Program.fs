open System
open Npgsql
open Dapper.FSharp
open Dapper.FSharp.PostgreSQL
// register our optional F# types
OptionTypes.register ()

type Author =
    { id: Guid
      name: string
      email: string
      twitter_handle: string option }

// register our tables
let authorTable =
    // we can use this function to match tables
    // with different names to our record definitions
    table'<Author> "authors" |> inSchema "public"

let connstring =
    "Host=127.0.0.1;Username=admin;Password=Admin123;Database=simple_fsharp"
/// In normal circunstances you would write
/// `use! conn = new NpgsqlConnection(connString)`
/// but inside F# scripts we're not allowed for top declarations like this,
/// so we use let instead
let conn = new NpgsqlConnection(connstring)

// Generate two different authors
// one with an optional handle to see how we can deal with null values
let authors =
    [ { id = Guid.NewGuid()
        name = "Angel D. Munoz"
        email = "some@email.com"
        twitter_handle = Some "angel_d_munoz" }
      { id = Guid.NewGuid()
        name = "Misterious Person"
        email = "mistery@email.com"
        twitter_handle = None } ]

// If you were to use ASP.NET core
// you would be running on a task or async method
task {
    /// the `!` here indicates that we will wait
    /// for the `InsertAsync` operation to finish
    let! result =
        // here's the Dapper.FSharp magical DSL
        insert {
            into authorTable
            values authors
        }
        |> conn.InsertAsync

    /// If all goes well you shoul'd see
    /// `Rows Affected: 2` in tour console
    printfn $"Rows Affected: %i{result}"

}
// we're inside a script hence why we need run it synchronously
// most of the time you don't need this
|> Async.AwaitTask
|> Async.RunSynchronously

//Finish Insert

let dd =
    task {
        return!
            select {
                for author in authorTable do
                    where (author.name = "Angel D. Munoz")
            }
            |> conn.SelectAsync<Author>
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously

printfn $"Authors: %A{dd}"
