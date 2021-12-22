namespace Web
open FSharp.Json
open System.Text.Json
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http
module Endpoints =
  let mutable store = 
    Map [ ("hello", "world") ]
  
  let get a =
    try
      let _, resp = store.TryGetValue(a)
      if isNull resp
      then "Not Found"
      else resp
    with
      | error -> error.ToString()
  
  let add (a:string, b:string, logger:ILogger) : string =
    logger.LogInformation(b)
    try
      store <- store.Add (a,b)
      "OK"
    with
      | error -> error.ToString()

  let addPost (body:JsonElement) (logger:ILogger) =
    try
      let newBody = body.Deserialize<Map<string,string>>()
      let keys = newBody.Keys
      let values = newBody.Values
      for key in keys do
        store <- store.Add(key.ToString(),newBody.Item(key).ToString())
      "OK"
    with
    | error -> error.ToString()

  let del a =
    store <- store.Remove(a)
    let res, _ = store.TryGetValue(a)
    if res
    then "Error: Value not removed! 👀"
    else "OK"

  let purge a =
    store <- Map[]
    "OK"

  let contents a =
    let mutable keys = store.Keys
    Json.serialize(store)