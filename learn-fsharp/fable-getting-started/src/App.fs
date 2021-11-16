module App
open Browser.Dom

let runAfter (ms:int) callback =
    async{
        do! Async.Sleep ms
        do callback()
    }|> Async.StartImmediate

let increase = document.getElementById "increase"
let decrease = document.getElementById "decrease"
let increaseDelayed = document.getElementById "increaseDelayed"
let counterViewer = document.getElementById "counterView"

let mutable curCount = 0

increase.onclick <- fun _ ->
  curCount <- curCount + 1
  counterViewer.innerText <- sprintf "Count is at %d" curCount

decrease.onclick <- fun _ ->
  curCount <- curCount - 1
  counterViewer.innerText <- sprintf "Count is at %d" curCount

increaseDelayed.onclick<-fun _->
    runAfter(1000) (fun () ->
        curCount <- curCount + 1
        counterViewer.innerText <- sprintf "Count is at %d" curCount)

counterViewer.innerText <- sprintf "Count is at %d" curCount