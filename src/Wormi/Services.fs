namespace Wormi.Services

open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.JSInterop
open IcedTasks
open System




module TextArea =
  type ITextAreaService =

    abstract actOnTxtArea:
      selector: string * payload: string -> ValueTask<string>


  let factory (services: IServiceProvider) =
    let js = services.GetRequiredService<IJSRuntime>()

    let jsModule =
      lazy
        (js.InvokeAsync<IJSObjectReference>("import", Wormi.ModuleNames.Library))

    { new ITextAreaService with
        member _.actOnTxtArea(selector, payload) = valueTask {
          let! db = jsModule.Value

          return!
            db.InvokeAsync<string>(
              Wormi.JS.Library.ExportedNames.ActOnTextArea,
              selector,
              payload
            )
        }

      interface IAsyncDisposable with
        member _.DisposeAsync() = valueTaskUnit {
          let! db = jsModule.Value
          return! db.DisposeAsync()
        }
    }

module LocalStorage =
  open Microsoft.Extensions.Logging

  type ILocalStorageService =

    abstract member ExtractFromStorage: unit -> ValueTask<string voption>
    abstract member SaveToStorage: string -> ValueTask

  let factory (services: IServiceProvider) =
    let js = services.GetRequiredService<IJSRuntime>()
    let logger = services.GetRequiredService<ILogger<ILocalStorageService>>()

    let jsModule =
      lazy
        (js.InvokeAsync<IJSObjectReference>("import", Wormi.ModuleNames.Browser))

    { new ILocalStorageService with
        member _.ExtractFromStorage() = valueTask {
          let! db = jsModule.Value
          logger.LogDebug("Extracting from storage")

          try

            let! result =
              db.InvokeAsync<string voption>(
                Wormi.JS.Browser.ExportedNames.ExtractFromStorage
              )

            logger.LogDebug("Extracted from storage: {result}", result)
            return result
          with ex ->
            logger.LogError("Failed to extract from storage: {ex}", ex)
            return! ValueTask.FromResult(ValueNone)
        }

        member _.SaveToStorage(value: string) = valueTaskUnit {
          let! db = jsModule.Value

          do!
            db.InvokeVoidAsync(
              Wormi.JS.Browser.ExportedNames.SaveToStorage,
              value
            )
        }

      interface IAsyncDisposable with
        member _.DisposeAsync() = valueTaskUnit {
          let! db = jsModule.Value
          return! db.DisposeAsync()
        }
    }


module Markdown =
  open Markdig

  type IMarkdownRenderer =
    abstract member RenderHtml: string -> string

  type Markdown =
    static member inline factory(?pipeline) =
      let pipeline =
        defaultArg
          pipeline
          (MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseSmartyPants()
            .Build())

      { new IMarkdownRenderer with
          member _.RenderHtml(markdown: string) =
            Markdown.ToHtml(markdown, pipeline)
      }
