open System

type LoginModel = { Username: string; Password: string }

let adminList =
    [ { Username = "Tom"; Password = "123" }
      { Username = "Jerry"; Password = "456" } ]
    |> Set.ofList

let rec LoginRoute () =
    printfn "Please input your Username"
    let username = Console.ReadLine()
    printfn "Please input your Password"
    let pwd = Console.ReadLine()
    let account = { Username = username; Password = pwd }

    match adminList |> Set.contains account with
    | true -> printfn "Ok!!!"
    | false ->
        printfn "wrong pwd or username!!"
        LoginRoute()

LoginRoute()
