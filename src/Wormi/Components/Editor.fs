namespace Wormi.Components

open System
open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Components.Forms
open Microsoft.Extensions.Logging
open FSharp.Data.Adaptive
open Fun.Blazor

open Wormi
open Wormi.Services.TextArea
open Wormi.Services.Markdown

open Thoth.Json.Net

type EditorModel = {
  title: string
  slug: string
  content: string
  tags: string list
}


module Editor =

  let HasChangesNotice (hasChanges: bool) = section {
    class' "editor-notice"

    region {
      if hasChanges then
        "You have unsaved changes"
      else
        String.Empty
    }
  }


  let PostTitle (title: string, setTitle: string -> unit) = input {
    class' "editor-title"
    value title
    oninput (fun e -> setTitle (unbox<string> e.Value))
  }

  let LivePreview (markdown: IMarkdownRenderer, postContent: string) = article {
    class' "preview"
    childContentRaw (markdown.RenderHtml postContent)
  }

type Editor =

  static member Editor
    (
      post: Post aval,
      ?markAsDraft: Post -> unit,
      ?markDirty: bool -> unit
    ) =
    let markDirty = defaultArg markDirty ignore
    let markAsDraft = defaultArg markAsDraft ignore

    html.comp
      (fun
           (markdown: IMarkdownRenderer,
            txtAreaService: ITextAreaService,
            hook: IComponentHook,
            logger: ILogger<Editor>) ->
        let editorForm =
          let slugTags =
            post
            |> AVal.map (fun post ->
              post.metadata
              |> ValueOption.map (fun m -> m.slug, m.tags)
              |> ValueOption.defaultValue ("", []))

          (post, slugTags)
          ||> AVal.map2 (fun post (slug, tags) ->
            hook.UseAdaptiveForm<EditorModel, string>(
              {
                content = post.content
                title = post.title
                slug = slug
                tags = tags
              }
            ))

        editorForm
        |> AVal.force
        |> _.UseHasChanges()
        |> AVal.addLazyCallback (fun value -> markDirty value)
        |> hook.AddDispose

        article {
          class' "editor-main"

          header {
            adaptiview () {
              let! editorForm = editorForm
              let! hasChanges = editorForm.UseHasChanges()
              let! title, setTitle = editorForm.UseField(fun m -> m.title)
              Editor.HasChangesNotice hasChanges
              Editor.PostTitle(title, setTitle)
            }
          }


          main {
            Toolbar.Create(fun feature ->
              task {
                let payload =
                  Encode.toString 0 (EditorTextFeatures.Encoder feature)

                let! result =
                  txtAreaService.actOnTxtArea ("#post-editor", payload)

                logger.LogInformation(result)
                let eform = editorForm |> AVal.force
                let setContent = eform.UseFieldSetter(fun m -> m.content)
                setContent result
              }
              |> ignore)

            section {
              adaptiview () {
                let! post = post
                let! editorForm = editorForm

                let! postContent, setContent =
                  editorForm.UseField(fun m -> m.content)

                textarea {
                  id "post-editor"
                  class' "editor"
                  oninput (fun e -> setContent (unbox<string> e.Value))

                  onblur (fun _ ->
                    markAsDraft {
                      post with
                          content = postContent
                          title = editorForm.GetFieldValue(fun f -> f.title)
                    })

                  postContent
                }
              }
            }

          }

          adaptiview () {
            let! form = editorForm
            let! content, _ = form.UseField(fun f -> f.content)

            Editor.LivePreview(markdown, content)
          }
        })
