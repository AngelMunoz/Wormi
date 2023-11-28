namespace Wormi.Services

open System.Threading.Tasks
open Microsoft.JSInterop
open IcedTasks
open System


type IDatabaseService =
  abstract member GetDatabaseName: unit -> ValueTask<string>

type DatabaseService(js: IJSRuntime) =

  let jsModule =
    lazy (js.InvokeAsync<IJSObjectReference>("import", "./js/database.js"))

  interface IDatabaseService with
    member _.GetDatabaseName() = valueTask {
      let! db = jsModule.Value
      return! db.InvokeAsync<string>("GetDatabaseName")
    }

  interface IAsyncDisposable with
    member _.DisposeAsync() = valueTaskUnit {
      let! db = jsModule.Value
      return! db.DisposeAsync()
    }
