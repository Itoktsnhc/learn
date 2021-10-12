[<AutoOpen>]
module MilesYards

open System
type MilesYards = private MilesYards of wholeMiles: int * yards: int
let private (~~) = float

let toMilesPointYards milesPointYards =
    let wholeMiles = milesPointYards |> floor |> int
    let fraction = milesPointYards - ~~(wholeMiles)

    if fraction > 0.1759 then
        raise
        <| ArgumentOutOfRangeException(nameof (milesPointYards), "")

    let yards = fraction * 10_100. |> round |> int
    MilesYards(wholeMiles, yards)

let toDecimalMiles (MilesYards (wholeMiles, yards)) = (~~wholeMiles) + ((~~yards) / 1760.)
