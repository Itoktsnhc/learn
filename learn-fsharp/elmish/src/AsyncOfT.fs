module App

open Elmish
open Elmish.React
open Feliz

type State = { Count: int; Loading: bool }

type Msg =
    | Increment
    | Decrement
    | IncrementDelayed
    | DecrementDelayed

let fromAsync (operation: Async<'msg>) : Cmd<'msg> =
    let delayedCmd (dispatch: 'msg -> unit) : unit =
        let delayedDispatch =
            async {
                let! msg = operation
                dispatch msg
            }

        Async.StartImmediate delayedDispatch

    Cmd.ofSub delayedCmd

let delayedMsg (delay: int) (msg: Msg) : Cmd<Msg> =
    let delay =
        async {
            do! Async.Sleep delay
            return msg
        }

    fromAsync delay

let init () =
    { Count = 0; Loading = false }, Cmd.none

let update msg state =
    match msg with
    | Increment ->
        { state with
              Count = state.Count + 1
              Loading = false },
        Cmd.none
    | Decrement ->
        { state with
              Count = state.Count - 1
              Loading = false },
        Cmd.none
    | IncrementDelayed when state.Loading -> state, Cmd.none
    | DecrementDelayed when state.Loading -> state, Cmd.none
    | IncrementDelayed -> state, delayedMsg 1000 Increment
    | DecrementDelayed -> state, delayedMsg 1000 Decrement


let render (state: State) (dispatch: Msg -> unit) =
    let content =
        if state.Loading then
            Html.h1 "LOADING..."
        else
            Html.h1 state.Count

    Html.div [ Html.button [ prop.onClick (fun _ -> dispatch Increment)
                             prop.text "Increment" ]
               Html.button [ prop.onClick (fun _ -> dispatch Decrement)
                             prop.text "Decrement" ]
               Html.button [ prop.onClick (fun _ -> dispatch IncrementDelayed)
                             prop.text "Increment Delayed"
                             prop.disabled state.Loading ]
               Html.button [ prop.onClick (fun _ -> dispatch DecrementDelayed)
                             prop.text "Decrement Delayed"
                             prop.disabled state.Loading ]

               Html.h1 content ]

Program.mkProgram init update render
|> Program.withReactSynchronous "elm-app"
|> Program.run
