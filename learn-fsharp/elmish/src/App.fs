module App

open Elmish
open Elmish.React
open Feliz

type State = { Count: int }

type Msg =
    | Increment
    | Decrement

let init () = { Count = 1 }

let update (msg: Msg) (state: State) : State =
    match msg with
    | Increment -> { state with Count = state.Count + 1 }

    | Decrement -> { state with Count = state.Count - 1 }

let render (state: State) (dispatch: Msg -> unit) =
    Html.div [ Html.button [ prop.onClick (fun _ -> dispatch Increment)
                             prop.text "+"
                             prop.classes [ "button"; "is-primary" ] ]
               Html.div state.Count
               Html.button [ prop.onClick (fun _ -> dispatch Decrement)
                             prop.text "-"
                             prop.classes [ "button"; "is-primary" ] ]
               Html.h1 [ prop.classes [ if state.Count < 0 then "hidden" ]
                         prop.text (
                             if state.Count % 2 = 0 then
                                 "Count is even"
                             else
                                 "Count is odd"
                         ) ] ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elm-app-bootstrap"
|> Program.run
