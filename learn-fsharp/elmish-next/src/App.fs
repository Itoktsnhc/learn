module App

open System
open Elmish
open Elmish.React
open Feliz

type StateFilter =
  |All
  |Completed
  |NotCompleted

type Todo = {
  Id : int
  Description : string
  Completed : bool
}

type TodoBeingEdited = {
  Id: int
  Description: string
}

type State = {
  Filter: StateFilter
  TodoList: Todo list
  NewTodo : string
  TodoBeingEdited : TodoBeingEdited option
}

type Msg =
  | SetNewTodo of string
  | AddNewTodo
  | DeleteTodo of int
  | ToggleCompleted of int
  | CancelEdit
  | ApplyEdit
  | StartEditingTodo of int
  | SetEditedDescription of string
  | ChangeFilter of StateFilter


let init() = {
  Filter= All
  TodoList = [
    { Id = 1; Description = "Learn F#"; Completed = false }
    { Id = 2; Description = "Learn Elmish"; Completed = true }
  ]
  NewTodo = ""
  TodoBeingEdited = None
}

let update (msg: Msg) (state: State) =
  match msg with
  | SetNewTodo desc ->
      { state with NewTodo = desc }

  | AddNewTodo when String.IsNullOrWhiteSpace state.NewTodo ->
      state

  | AddNewTodo ->
      let nextTodoId =
        match state.TodoList with
        | [ ] -> 1
        | elems ->
            elems
            |> List.maxBy (fun todo -> todo.Id)
            |> fun todo -> todo.Id + 1

      let nextTodo =
        { Id = nextTodoId
          Description = state.NewTodo
          Completed = false }

      { state with
          NewTodo = ""
          TodoList = List.append state.TodoList [nextTodo] }

  | DeleteTodo todoId ->
      let nextTodoList =
        state.TodoList
        |> List.filter (fun todo -> todo.Id <> todoId)

      { state with TodoList = nextTodoList }

  | ToggleCompleted todoId ->
      let nextTodoList =
        state.TodoList
        |> List.map (fun todo ->
           if todo.Id = todoId
           then { todo with Completed = not todo.Completed }
           else todo)

      { state with TodoList = nextTodoList }

  | StartEditingTodo todoId ->
      let nextEditModel =
        state.TodoList
        |> List.tryFind (fun todo -> todo.Id = todoId)
        |> Option.map (fun todo -> { Id = todoId; Description = todo.Description })

      { state with TodoBeingEdited = nextEditModel }

  | CancelEdit ->
      { state with TodoBeingEdited = None }

  | ApplyEdit ->
      match state.TodoBeingEdited with
      | None -> state
      | Some todoBeingEdited when todoBeingEdited.Description = "" -> state
      | Some todoBeingEdited ->
          let nextTodoList =
            state.TodoList
            |> List.map (fun todo ->
                if todo.Id = todoBeingEdited.Id
                then { todo with Description = todoBeingEdited.Description }
                else todo)

          { state with TodoList = nextTodoList; TodoBeingEdited = None }

  | SetEditedDescription newText ->
      let nextEditModel =
        state.TodoBeingEdited
        |> Option.map (fun todoBeingEdited -> { todoBeingEdited with Description = newText })

      { state with TodoBeingEdited = nextEditModel }
  | ChangeFilter filter -> {state with Filter = filter}

// Helper function to easily construct div with only classes and children
let div (classes: string list) (children: ReactElement list) =
    Html.div [
        prop.classes classes
        prop.children children
    ]

let appTitle =
    Html.p [
      prop.className "title"
      prop.text "Elmish To-Do List"
    ]

let inputField (state: State) (dispatch: Msg -> unit) =
  div [ "field"; "has-addons" ] [
    div [ "control"; "is-expanded" ] [
      Html.input [
        prop.classes [ "input"; "is-medium" ]
        prop.valueOrDefault state.NewTodo
        prop.onTextChange (SetNewTodo >> dispatch)
      ]
    ]

    div [ "control" ] [
      Html.button [
        prop.classes [ "button"; "is-primary"; "is-medium" ]
        prop.onClick (fun _ -> dispatch AddNewTodo)
        prop.children [
          Html.i [ prop.classes [ "fa"; "fa-plus" ] ]
        ]
      ]
    ]
  ]

let renderTodo (todo: Todo) (dispatch: Msg -> unit) =
  div [ "box" ] [
    div [ "columns"; "is-mobile"; "is-vcentered" ] [
      div [ "column"; "subtitle"] [
        Html.p [
          prop.className "subtitle"
          prop.text todo.Description
        ]
      ]

      div [ "column"; "is-narrow" ] [
        div [ "buttons" ] [
          Html.button [
            prop.className [ true, "button"; todo.Completed, "is-success"]
            prop.onClick (fun _ -> dispatch (ToggleCompleted todo.Id))
            prop.children [
              Html.i [ prop.classes [ "fa"; "fa-check" ] ]
            ]
          ]

          Html.button [
            prop.classes [ "button"; "is-primary" ]
            prop.onClick (fun _ -> dispatch (StartEditingTodo  todo.Id))
            prop.children [
              Html.i [ prop.classes [ "fa"; "fa-edit" ] ]
            ]
          ]

          Html.button [
            prop.classes [ "button"; "is-danger" ]
            prop.onClick (fun _ -> dispatch (DeleteTodo todo.Id))
            prop.children [
              Html.i [ prop.classes [ "fa"; "fa-times" ] ]
            ]
          ]
        ]
      ]
    ]
  ]


let renderEditForm (todoBeingEdited: TodoBeingEdited) (dispatch: Msg -> unit) =
  div [ "box" ] [
    div [ "field is-grouped" ] [
      div [ "control is-expanded" ] [
        Html.input [
          prop.classes [ "input"; "is-medium" ]
          prop.valueOrDefault todoBeingEdited.Description;
          prop.onTextChange (SetEditedDescription >> dispatch)
        ]
      ]

      div [ "control"; "buttons" ] [
        Html.button [
          prop.classes [ "button"; "is-primary"]
          prop.onClick (fun _ -> dispatch ApplyEdit)
          prop.children [
            Html.i [ prop.classes ["fa"; "fa-save" ] ]
          ]
        ]

        Html.button [
          prop.classes ["button"; "is-warning"]
          prop.onClick (fun _ -> dispatch CancelEdit)
          prop.children [
            Html.i [ prop.classes ["fa"; "fa-arrow-right"] ]
          ]
        ]
      ]
    ]
  ]

let tabFilter (state: State) (dispatch: Msg->unit) =
  Html.div [
    prop.className "tabs"
    prop.children [
      Html.ul [
        prop.children [
          Html.li [
            prop.children[Html.a [Html.text "ALL"]]
            prop.className [state.Filter= All, "is-active"; ]
            prop.onClick (fun _ -> dispatch (ChangeFilter All))
          ]
          Html.li [
            prop.children[Html.a [Html.text "Completed"]]
            prop.className [state.Filter = Completed, "is-active"]
            prop.onClick (fun _ -> dispatch (ChangeFilter Completed))
          ]
          Html.li [
            prop.children[Html.a [Html.text "Not Completed"]]
            prop.className [state.Filter = NotCompleted,"is-active"]
            prop.onClick (fun _ -> dispatch (ChangeFilter NotCompleted))
          ]
        ]
      ]
    ]
  ]

let filterOut (state: State) (todo:Todo) =
  match state.Filter with
  |All -> true
  |Completed -> todo.Completed
  |NotCompleted -> not todo.Completed

let todoList (state: State) (dispatch: Msg -> unit) =
  Html.ul [
    prop.children [
      for todo in state.TodoList|>List.filter (fun x ->filterOut state x) ->
        match state.TodoBeingEdited with
        | Some todoBeingEdited when todoBeingEdited.Id = todo.Id ->
            renderEditForm todoBeingEdited dispatch
        | otherwise ->
            renderTodo todo dispatch
    ]
  ]

let render (state: State) (dispatch: Msg -> unit) =
  Html.div [
    prop.style [ style.padding 20 ]
    prop.children [
      appTitle
      inputField state dispatch
      tabFilter state dispatch
      todoList state dispatch
    ]
  ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run