﻿// <auto-generated />
namespace Console.Migrations

open System
open Console
open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Infrastructure
open Microsoft.EntityFrameworkCore.Metadata
open Microsoft.EntityFrameworkCore.Migrations
open Microsoft.EntityFrameworkCore.Storage.ValueConversion

[<DbContext(typeof<BloggingModel.BloggingContext>)>]
[<Migration("20211221054356_InitCreate")>]
type InitCreate() =
    inherit Migration()

    override this.Up(migrationBuilder:MigrationBuilder) =
        migrationBuilder.CreateTable(
            name = "Blogs"
            ,columns = (fun table -> 
            {|
                Id =
                    table.Column<int>(
                        nullable = false
                        ,``type`` = "INTEGER"
                    ).Annotation("Sqlite:Autoincrement", true)
                Url =
                    table.Column<string>(
                        nullable = true
                        ,``type`` = "TEXT"
                    )
            |})
            , constraints =
                (fun table -> 
                    table.PrimaryKey("PK_Blogs", (fun x -> (x.Id) :> obj)
                    ) |> ignore
                )
        ) |> ignore


    override this.Down(migrationBuilder:MigrationBuilder) =
        migrationBuilder.DropTable(
            name = "Blogs"
            ) |> ignore


    override this.BuildTargetModel(modelBuilder: ModelBuilder) =
        modelBuilder.HasAnnotation("ProductVersion", "6.0.1") |> ignore

        modelBuilder.Entity("Console.BloggingModel+Blog", (fun b ->

            b.Property<int>("Id")
                .IsRequired(true)
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER")
                .HasDefaultValue(0)
                |> ignore

            b.Property<string>("Url")
                .IsRequired(true)
                .HasColumnType("TEXT")
                |> ignore

            b.HasKey("Id")
                |> ignore


            b.ToTable("Blogs") |> ignore

        )) |> ignore
