namespace Wormi.Pages


open System.Threading.Tasks

open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Components.Routing
open Microsoft.Extensions.Logging

open IcedTasks
open FSharp.Data.Adaptive
open Fun.Blazor

open Thoth.Json.Net

open Wormi
open Wormi.Components
open Wormi.Services.LocalStorage
open System

module Home =
  let LoadDraft
    (localStorage: ILocalStorageService, logger: ILogger)
    (newPost: Post cval)
    =
    taskUnit {
      let! draft = localStorage.ExtractFromStorage()
      logger.LogDebug("Storage Content: {draft}", draft)

      let draft =
        draft |> ValueOption.map (Decode.fromString LocalDraft.Decoder)

      match draft with
      | ValueSome(Ok draft) ->
        logger.LogDebug("Updating from local storge: {draft}", draft)

        newPost.Publish(fun post -> {
          post with
              content = draft.content
              title = draft.title
        })
      | ValueSome(Error err) ->
        logger.LogError(
          "Failed to decode draft from local storage: {error}",
          err
        )
      | ValueNone -> logger.LogDebug("No draft found in local storage")
    }

  let SaveDraft (localStorage: ILocalStorageService) (post: Post) =
    valueTaskUnit {
      let draft = {
        title = post.title
        content = post.content
      }


      do!
        Encode.toString 0 (LocalDraft.Encoder draft)
        |> localStorage.SaveToStorage
    }
    |> ignore

[<Route "/">]
type Home() =
  inherit FunBlazorComponent()

  let newPost =
    cval {
      _id = System.Guid.NewGuid().ToString()
      _rev = ValueNone
      title = ""
      content = ""
      status = PostStatus.Draft
      metadata = ValueNone
      updated_at = ValueNone
      created_at = DateTime.Now
    }

  [<Inject>]
  member val Logger: ILogger<Home> = Unchecked.defaultof<_> with get, set

  [<Inject>]
  member val LocalStorage: ILocalStorageService =
    Unchecked.defaultof<_> with get, set

  override self.OnInitializedAsync() =
    let loadDraft = Home.LoadDraft(self.LocalStorage, self.Logger)
    loadDraft newPost


  override self.Render() =
    let saveDraft = Home.SaveDraft(self.LocalStorage)

    article {
      class' "page home"
      Navbar.Create()

      main {
        section {
          class' "home-editor"

          Editor.Editor(
            newPost,
            markAsDraft =
              (fun newContent ->
                self.Logger.LogInformation(
                  "Saving draft: {content}",
                  newContent
                )

                saveDraft newContent),
            markDirty =
              (fun isDirty ->
                self.Logger.LogInformation("It Changed: {isDirty}", isDirty))
          )
        }
      }
    }
