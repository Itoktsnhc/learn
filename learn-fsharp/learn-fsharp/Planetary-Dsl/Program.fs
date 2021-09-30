// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open DSL

[<EntryPoint>]
let main argv =
    let str =
        "PlanetarySystem 'Solar'
    Star 'Sol' 7e8 2e30 
    Planet 'Mercury' 2440000  3e23 
    Planet 'Venus'   6052000  5e24 
    Planet 'Earth'   6378000  6e24
    Planet 'Mars'    3397000  6e23
    Planet 'Jupiter' 71492000  2e27
    Planet 'Saturn'  60268000  6e26
    Planet 'Uranus'  25559000  9e25 
    Planet 'Neptune' 24766000  1e26
    Planet 'Pluto'   1150000  1e22"

    let info =
        PlanetarySystemParser.createPlanetarySystem (str)

    printf $"%A{info}"
    0
