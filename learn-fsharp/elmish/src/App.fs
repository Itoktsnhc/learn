module App

open Elmish
open Elmish.React
open Feliz
open Validation

type State = { NumberInput: Validated<int> }
type Msg = SetNumberInput of Validated<int>

let init () = { NumberInput = createEmpty () }

let update msg state =
    match msg with
    | SetNumberInput numberInput -> { state with NumberInput = numberInput }

let tryParseInt (input: string) : Validated<int> =
    try
        success input (int input)
    with
    | _ -> failure input

let validatedTextColor validated =
    match validated.Parsed with
    | Some _ -> color.green
    | None -> color.crimson

let render state dispatch =
    Html.div [ prop.style [ style.padding 20 ]
               prop.children [ Html.input [ prop.valueOrDefault state.NumberInput.Raw
                                            prop.onChange (tryParseInt >> SetNumberInput >> dispatch) ]
                               Html.h1 [ prop.style [ style.color (validatedTextColor state.NumberInput) ]
                                         prop.text state.NumberInput.Raw ] ] ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elm-app-bootstrap"

|> Program.run
