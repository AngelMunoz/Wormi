namespace Wormi.Components

open System
open Microsoft.AspNetCore.Components
open FSharp.Data.Adaptive
open Fun.Blazor

open Wormi

module Markdown =
  open Markdig

  let pipeline =
    lazy
      (MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UseSmartyPants()
        .Build())

  let ToHtml (markdown: string) =
    Markdown.ToHtml(markdown, pipeline.Value)


module Editor =



  let Toolbar () = nav { "toolbar" }

  let Editor (post: Post) =
    let initial = post.content
    let content = cval (post.content)

    adaptiview () {
      let! text, setText = content.WithSetter()
      let! dirty = content |> AVal.map (fun content -> content <> initial)

      article {
        link {
          rel "stylesheet"
          stylesheet "/editor.css"
        }

        header {
          region {
            if dirty then
              section { "There are unsaved changes." }
          }
        }

        main {
          class' "editor-main"

          section {
            textarea {
              class' "editor"
              oninput (fun e -> setText (e.Value :?> string))
            }
          }

          section {
            class' "preview"
            childContentRaw (Markdown.ToHtml(text))
          }
        }




      }

    }
