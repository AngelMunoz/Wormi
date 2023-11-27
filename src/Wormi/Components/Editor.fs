namespace Wormi.Components

open FSharp.Data.Adaptive
open Fun.Blazor

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

  let Editor (initial: string) = adaptiview () {
    let! text, setText = cval(initial).WithSetter()

    textarea { oninput (fun e -> setText (e.Value :?> string)) }

    article { childContentRaw (Markdown.ToHtml(text)) }
  }
