module App

open Elmish
open Elmish.React
open Feliz

type State =
    { TodoList: string list
      NewTodo: string }

type Msg =
    | SetNewTodo of string
    | AddNewTodo

let init () =
    { TodoList = [ "Learn F#" ]
      NewTodo = "" }

let update msg state =
    match msg with
    | SetNewTodo todoText -> { state with NewTodo = todoText }
    | AddNewTodo when state.NewTodo = "" -> state
    | AddNewTodo ->
        { state with
              NewTodo = ""
              TodoList = List.append state.TodoList [ state.NewTodo ] }

let appTitle =
    Html.p [ prop.className "title"
             prop.text "Elm To-Do List" ]

let inputField (state: State) (dispatch: Msg -> unit) =
    Html.div [ prop.classes [ "field"; "has-addons" ]
               prop.children [ Html.div [ prop.classes [ "control"
                                                         "is-expanded" ]
                                          prop.children [ Html.input [ prop.classes [ "input"; "is-medium" ]
                                                                       prop.valueOrDefault state.NewTodo
                                                                       prop.onChange (SetNewTodo >> dispatch) ] ] ]

                               Html.div [ prop.className "control"
                                          prop.children [ Html.button [ prop.classes [ "button"
                                                                                       "is-primary"
                                                                                       "is-medium" ]
                                                                        prop.onClick (fun _ -> dispatch AddNewTodo)
                                                                        prop.children [ Html.i [ prop.classes [ "fa"
                                                                                                                "fa-plus" ] ] ] ] ] ] ] ]

let todoList (state: State) (dispatch: Msg -> unit) =
    Html.ul [ prop.children [ for todo in state.TodoList ->
                                  Html.li [ prop.classes [ "box"; "subtitle" ]
                                            prop.text todo ] ] ]

let render (state: State) (dispatch: Msg -> unit) =
    Html.div [ prop.style [ style.padding 20 ]
               prop.children [ appTitle
                               inputField state dispatch
                               todoList state dispatch ] ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elm-app"

|> Program.run
