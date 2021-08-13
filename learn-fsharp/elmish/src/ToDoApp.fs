module App

open Elmish
open Elmish.React
open Feliz
open System

type Todo =
    { Id: Guid
      Description: string
      Completed: bool }

type TodoBeingEdited = { Id: Guid; Description: string }

type FilterVal =
    | All
    | Completed
    | NotCompleted


type State =
    { TodoList: Todo list
      RawTodoList: Todo list
      NewTodo: string
      TodoBeingEdited: TodoBeingEdited option
      TabFilter: FilterVal }



type Msg =
    | SetNewTodo of string
    | AddNewTodo
    | DeleteTodo of Guid
    | ToggleCompleted of Guid
    | CancelEdit
    | ApplyEdit
    | StartEditingTodo of Guid
    | SetEditedDescription of string
    | SetTableFilter of FilterVal


let finished =
    { Id = Guid.NewGuid()
      Description = "Learn Elmish"
      Completed = true }

let notFinished =
    { Id = Guid.NewGuid()
      Description = "Learn F#"
      Completed = false }

let defaultList: (Todo list) = [ finished; notFinished ]

let init () =
    { TodoList = defaultList
      RawTodoList = defaultList
      NewTodo = ""
      TodoBeingEdited = None
      TabFilter = All }

let filterState state (todo: Todo) =
    match state.TabFilter, todo.Completed with
    | All, _ -> true
    | Completed, true -> true
    | NotCompleted, false -> true
    | _ -> false

let update (msg: Msg) (state: State) =
    match msg with
    | SetNewTodo desc -> { state with NewTodo = desc }

    | AddNewTodo when String.IsNullOrWhiteSpace state.NewTodo -> state

    | AddNewTodo ->
        let nextTodo =
            { Id = Guid.NewGuid()
              Description = state.NewTodo
              Completed = false }

        let nextTodoList = List.append state.TodoList [ nextTodo ]

        { state with
              NewTodo = ""
              TodoList = nextTodoList |> List.filter (filterState state)
              RawTodoList = nextTodoList }

    | DeleteTodo todoId ->
        let nextTodoList =
            state.TodoList
            |> List.filter (fun todo -> todo.Id <> todoId)

        { state with TodoList = nextTodoList }

    | ToggleCompleted todoId ->
        let nextTodoList =
            state.TodoList
            |> List.map
                (fun todo ->
                    if todo.Id = todoId then
                        { todo with
                              Completed = not todo.Completed }
                    else
                        todo)

        { state with
              RawTodoList = nextTodoList
              TodoList = nextTodoList |> List.filter (filterState state) }

    | StartEditingTodo todoId ->
        let nextEditModel =
            state.TodoList
            |> List.tryFind (fun todo -> todo.Id = todoId)
            |> Option.map
                (fun todo ->
                    { Id = todoId
                      Description = todo.Description })

        { state with
              TodoBeingEdited = nextEditModel }

    | CancelEdit -> { state with TodoBeingEdited = None }

    | ApplyEdit ->
        match state.TodoBeingEdited with
        | None -> state
        | Some todoBeingEdited when todoBeingEdited.Description = "" -> state
        | Some todoBeingEdited ->
            let nextTodoList =
                state.TodoList
                |> List.map
                    (fun todo ->
                        if todo.Id = todoBeingEdited.Id then
                            { todo with
                                  Description = todoBeingEdited.Description }
                        else
                            todo)

            { state with
                  TodoList = nextTodoList |> List.filter (filterState state)
                  RawTodoList = nextTodoList
                  TodoBeingEdited = None }

    | SetEditedDescription newText ->
        let nextEditModel =
            state.TodoBeingEdited
            |> Option.map
                (fun todoBeingEdited ->
                    { todoBeingEdited with
                          Description = newText })

        { state with
              TodoBeingEdited = nextEditModel }
    | SetTableFilter filter ->
        { state with
              TabFilter = filter
              TodoList =
                  state.RawTodoList
                  |> List.filter
                      (fun x ->
                          match filter with
                          | All -> true
                          | Completed -> x.Completed
                          | NotCompleted -> not x.Completed) }


// Helper function to easily construct div with only classes and children
let div (classes: string list) (children: ReactElement list) =
    Html.div [ prop.classes classes
               prop.children children ]

let appTitle =
    Html.p [ prop.className Bulma.Title
             prop.text "Elmish To-Do List" ]

let inputField (state: State) (dispatch: Msg -> unit) =
    div [ Bulma.Field; Bulma.HasAddons ] [
        div [ Bulma.Control; Bulma.IsExpanded ] [
            Html.input [ prop.classes [ Bulma.Input
                                        Bulma.IsMedium ]
                         prop.valueOrDefault state.NewTodo
                         prop.onTextChange (SetNewTodo >> dispatch) ]
        ]

        div [ "control" ] [
            Html.button [ prop.classes [ Bulma.Button
                                         Bulma.IsPrimary
                                         Bulma.IsMedium ]
                          prop.onClick (fun _ -> dispatch AddNewTodo)
                          prop.children [ Html.i [ prop.classes [ Bulma.Fa; FA.FaPlus ] ] ] ]
        ]
    ]

let renderTodo (todo: Todo) (dispatch: Msg -> unit) =
    div [ Bulma.Box ] [
        div [ Bulma.Columns
              Bulma.IsMobile
              Bulma.IsVcentered ] [
            div [ Bulma.Column; Bulma.Subtitle ] [
                Html.p [ prop.className Bulma.Subtitle
                         prop.text todo.Description ]
            ]

            div [ Bulma.Column; Bulma.IsNarrow ] [
                div [ Bulma.Buttons ] [
                    Html.button [ prop.className [ true, Bulma.Button
                                                   todo.Completed, Bulma.IsSuccess ]
                                  prop.onClick (fun _ -> dispatch (ToggleCompleted todo.Id))
                                  prop.children [ Html.i [ prop.classes [ Bulma.Fa; FA.FaCheck ] ] ] ]

                    Html.button [ prop.classes [ Bulma.Button
                                                 Bulma.IsPrimary ]
                                  prop.onClick (fun _ -> dispatch (StartEditingTodo todo.Id))
                                  prop.children [ Html.i [ prop.classes [ Bulma.Fa; FA.FaEdit ] ] ] ]

                    Html.button [ prop.classes [ Bulma.Button
                                                 Bulma.IsDanger ]
                                  prop.onClick (fun _ -> dispatch (DeleteTodo todo.Id))
                                  prop.children [ Html.i [ prop.classes [ Bulma.Fa; FA.FaTimes ] ] ] ]
                ]
            ]
        ]
    ]

let renderEditForm (todoBeingEdited: TodoBeingEdited) (dispatch: Msg -> unit) =
    div [ Bulma.Box ] [
        div [ Bulma.Field; Bulma.IsGrouped ] [
            div [ Bulma.Control; Bulma.IsExpanded ] [
                Html.input [ prop.classes [ Bulma.Input
                                            Bulma.IsMedium ]
                             prop.valueOrDefault todoBeingEdited.Description
                             prop.onTextChange (SetEditedDescription >> dispatch) ]
            ]

            div [ Bulma.Control; Bulma.Buttons ] [
                Html.button [ prop.classes [ Bulma.Button
                                             Bulma.IsPrimary ]
                              prop.disabled (todoBeingEdited.Description = "")
                              prop.onClick (fun _ -> dispatch ApplyEdit)
                              prop.children [ Html.i [ prop.classes [ Bulma.Fa; "fa-save" ] ] ] ]

                Html.button [ prop.classes [ Bulma.Button
                                             Bulma.IsWarning ]
                              prop.onClick (fun _ -> dispatch CancelEdit)
                              prop.children [ Html.i [ prop.classes [ Bulma.Fa
                                                                      "fa-arrow-right" ] ] ] ]
            ]
        ]
    ]

let todoList (state: State) (dispatch: Msg -> unit) =
    Html.ul [ prop.children [ for todo in state.TodoList ->
                                  match state.TodoBeingEdited with
                                  | Some todoBeingEdited when todoBeingEdited.Id = todo.Id -> renderEditForm todoBeingEdited dispatch
                                  | _ -> renderTodo todo dispatch ] ]

let renderFilterTabs (state: State) (dispatch: Msg -> unit) =
    div [ "tabs"; "is-toggle"; "is-fullwidth" ] [
        Html.ul [ for tab in [ All; Completed; NotCompleted ] do
                      Html.li [ if state.TabFilter = tab then
                                    prop.className "is-active"
                                prop.children [ Html.a [ prop.text (tab.ToString()) ] ]
                                prop.onClick (fun _ -> dispatch (SetTableFilter tab)) ] ]
    ]




let render (state: State) (dispatch: Msg -> unit) =
    Html.div [ prop.style [ style.padding 20 ]
               prop.children [ appTitle
                               inputField state dispatch
                               renderFilterTabs state dispatch
                               todoList state dispatch ] ]

// Program.mkSimple init update render
// |> Program.withReactSynchronous "elm-app"
// |> Program.run
