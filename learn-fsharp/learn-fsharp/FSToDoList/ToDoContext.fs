namespace FSToDoList

open FSToDoList.Models
open Microsoft.EntityFrameworkCore
open System.Linq

module DataContext =
    type ToDoContext(options: DbContextOptions<ToDoContext>) =
        inherit DbContext(options)

        [<DefaultValue>]
        val mutable ToDoItems: DbSet<ToDoItem>

        member this._ToDoItems
            with public get () = this.ToDoItems
            and public set value = this.ToDoItems <- value

    