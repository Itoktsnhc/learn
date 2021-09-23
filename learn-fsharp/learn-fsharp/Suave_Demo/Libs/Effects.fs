module Suave_Demo.Libs.Effects

open MongoDB.Driver
open Suave_Demo.Libs.Models.Models



module DB =
    open System.IO
    open MongoDB.Bson
    open System
    open Microsoft.Extensions.Configuration

    let curDir = Directory.GetCurrentDirectory()

    let getConfigDbConnection currDir =
        ConfigurationBuilder()
            .SetBasePath(currDir)
            .AddJsonFile("appsettings.json")
            .Build()

    let getPassPhrase () =
        (getConfigDbConnection curDir)
            .GetValue<string>("jwtPassPhrase")

    let getSavedTagList (dbClient: IMongoDatabase) =
        let collection = dbClient.GetCollection<TagCloud>("Tags")
        let numberOfTagDocs = collection.AsQueryable().ToList().Count

        if numberOfTagDocs > 0 then
            Some(collection.AsQueryable().First())
        else
            None

    let getSavedArticles (dbClient: IMongoDatabase) (query: string) (options: ArticleQueryOption) =
        let collection =
            dbClient.GetCollection<BsonDocument>("Article")

        let filter =
            FilterDefinition<BsonDocument>.op_Implicit (query)

        let articleList =
            match options with
            | Limit amount ->
                collection
                    .Find(filter)
                    .Limit(Nullable<int>(amount))
                    .ToList()
                |> List.ofSeq
            | Offset amount ->
                collection
                    .Find(filter)
                    .Skip(Nullable<int>(amount))
                    .ToList()
                |> List.ofSeq
            | Neither -> collection.Find(filter).ToList() |> List.ofSeq

        if not (List.isEmpty articleList) then
            Some articleList
        else
            None
