namespace Wormi.Components

open System
open Microsoft.AspNetCore.Components
open FSharp.Data.Adaptive
open Fun.Blazor

open Wormi
open Wormi.Services.Markdown

type EditorModel = {
  title: string
  slug: string
  content: string
  tags: string list
}


module Editor =

  let HasChangesNotice (form: AdaptiveForm<EditorModel, string>) = adaptiview () {
    let! hasChanges = form.UseHasChanges()

    region {
      if hasChanges then
        "You have unsaved changes"
      else
        String.Empty
    }
  }

  let PostTitle (form: AdaptiveForm<EditorModel, string>) = adaptiview () {
    let! title, setTitle = form.UseField(fun m -> m.title)

    input {
      class' "editor-title"
      value title
      oninput (fun e -> setTitle (unbox<string> e.Value))
    }
  }

  let LivePreview (markdown: IMarkdownRenderer, postContent: string) = article {
    class' "preview"
    childContentRaw (markdown.RenderHtml postContent)
  }

type Editor =

  static member Editor
    (
      post: Post,
      ?markAsDraft: Post -> unit,
      ?markDirty: bool -> unit
    ) =
    let markDirty = defaultArg markDirty ignore
    let markAsDraft = defaultArg markAsDraft ignore

    html.comp (
      post._id,
      fun (markdown: IMarkdownRenderer, hook: IComponentHook) ->
        let editorForm =
          let slug, tags =
            post.metadata
            |> ValueOption.map (fun m -> m.slug, m.tags)
            |> ValueOption.defaultValue ("", [])

          hook.UseAdaptiveForm<EditorModel, string>(
            {
              content = post.content
              title = post.title
              slug = slug
              tags = tags
            }
          )

        editorForm.UseHasChanges()
        |> AVal.addLazyCallback (fun value -> markDirty value)
        |> hook.AddDispose

        article {
          class' "editor-main"

          header {
            h3 { Editor.HasChangesNotice editorForm }

            section { Editor.PostTitle editorForm }
          }

          adaptiview () {
            let! postContent, setContent =
              editorForm.UseField(fun m -> m.content)

            main {
              Toolbar.Create()

              section {
                textarea {
                  class' "editor"
                  oninput (fun e -> setContent (unbox<string> e.Value))

                  onblur (fun _ ->
                    markAsDraft { post with content = postContent })

                  postContent
                }
              }

            }

            Editor.LivePreview(markdown, postContent)

          }
        }
    )
