module Console.Program

open Console.BloggingModel

let dbContext = new BloggingContext()

let newBlog =
    { Id = 0
      Url = "http://blogs.msdn.com/adonet" }

dbContext.Blogs.Add(newBlog) |> ignore
dbContext.SaveChanges() |> ignore

dbc
