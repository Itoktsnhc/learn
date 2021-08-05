module App

open Elmish
open Elmish.React
open Feliz


type State = { TextInput: string }
type Msg = SetTextInput of string

let init () = { TextInput = "" }

let update msg state =
    match msg with
    | SetTextInput textInput -> { state with TextInput = textInput }

let render state dispatch =
    Html.div [ Html.input [ prop.onChange (SetTextInput >> dispatch) ]
               Html.span state.TextInput ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elm-app-bootstrap"

|> Program.run
