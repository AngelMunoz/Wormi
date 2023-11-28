namespace Wormi.Components

open System
open Microsoft.AspNetCore.Components
open FSharp.Data.Adaptive
open Fun.Blazor

open Wormi
open Wormi.Services.Markdown

type EditorModel = { content: string }


type Editor =


  static member Editor
    (
      post: Post,
      ?markAsDraft: string -> unit,
      ?markDirty: bool -> unit
    ) =
    let markDirty = defaultArg markDirty ignore
    let markAsDraft = defaultArg markAsDraft ignore

    html.comp (
      post._id,
      fun (markdown: IMarkdownRenderer, hook: IComponentHook) ->
        let editorForm =
          hook.UseAdaptiveForm<EditorModel, string>({ content = post.content })

        editorForm.UseHasChanges()
        |> AVal.addLazyCallback (fun value -> markDirty value)
        |> hook.AddDispose

        article {
          class' "editor-main"

          header {
            adaptiview () {
              let! hasChanges = editorForm.UseHasChanges()

              region {
                if hasChanges then
                  "You have unsaved changes"
                else
                  String.Empty
              }
            }
          }

          adaptiview () {
            let! postContent, setContent =
              editorForm.UseField(fun m -> m.content)

            section {
              textarea {
                class' "editor"
                oninput (fun e -> setContent (unbox<string> e.Value))
                onblur (fun _ -> markAsDraft postContent)
                postContent
              }
            }

            section {
              class' "preview"
              childContentRaw (markdown.RenderHtml postContent)
            }
          }
        }
    )
