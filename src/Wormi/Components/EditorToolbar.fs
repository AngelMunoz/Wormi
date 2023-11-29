namespace Wormi.Components

open System
open Microsoft.AspNetCore.Components
open FSharp.Data.Adaptive
open Fun.Blazor

[<Struct>]
type ToolbarFeature =
  | Undo
  | Redo
  | Bold
  | Italic
  | Strikethrough
  | Heading of int
  | Highlight
  | Indent
  | Outdent
  | List
  | Link
  | Quote
  | Image
  | Code
  | CodeBlock
  | HorizontalRule

type Toolbar =
  static member ToolbarButton
    (
      feature: ToolbarFeature,
      ?onFeature: ToolbarFeature -> unit,
      ?title: string,
      ?isEnabled: bool
    ) =
    let title = defaultArg title ""
    let isEnabled = defaultArg isEnabled true
    let onFeature = defaultArg onFeature ignore

    button {
      class' "editor-toolbar-button"
      disabled (not isEnabled)
      onclick (fun _ -> onFeature feature)

      region {
        // TODO: Use the correct SVG icons
        match feature with
        | Undo -> "↶"
        | Redo -> "↷"
        | Bold -> "B"
        | Italic -> "I"
        | Strikethrough -> "S"
        | Heading 1 -> "H1"
        | Heading 2 -> "H2"
        | Heading 3 -> "H3"
        | Heading 4 -> "H4"
        | Heading 5 -> "H5"
        | Heading 6 -> "H6"
        | Highlight -> "H"
        | Indent -> "→"
        | Outdent -> "←"
        | List -> "-"
        | Link -> "🔗"
        | Quote -> "“”"
        | Image -> "🖼"
        | Code -> "<>"
        | CodeBlock -> "```"
        | HorizontalRule -> "HR"
        | _ -> title
      }

      title
    }

  static member ToolbarButtonGroup
    (
      buttons: ToolbarFeature list,
      ?onFeature: ToolbarFeature -> unit,
      ?isEnabled: ToolbarFeature -> bool
    ) =
    let onFeature = defaultArg onFeature ignore
    let isEnabled = defaultArg isEnabled (fun _ -> true)

    ul {
      class' "editor-toolbar-button-group"

      for feature in buttons do
        li {
          Toolbar.ToolbarButton(
            feature,
            onFeature = onFeature,
            isEnabled = isEnabled feature
          )
        }
    }

  static member Create
    (
      ?onFeature: ToolbarFeature -> unit,
      ?isEnabled: ToolbarFeature -> bool
    ) =
    let onFeature = defaultArg onFeature ignore
    let isEnabled = defaultArg isEnabled (fun _ -> true)

    section {
      class' "editor-toolbar"

      Toolbar.ToolbarButtonGroup(
        [ Undo; Redo ],
        onFeature = onFeature,
        isEnabled = isEnabled
      )

      Toolbar.ToolbarButtonGroup(
        [ Bold; Italic; Strikethrough ],
        onFeature = onFeature,
        isEnabled = isEnabled
      )

      Toolbar.ToolbarButtonGroup(
        [ Heading 1; Heading 2; Heading 3; Heading 4; Heading 5; Heading 6 ],
        onFeature = onFeature,
        isEnabled = isEnabled
      )

      Toolbar.ToolbarButtonGroup(
        [ Highlight; Indent; Outdent; HorizontalRule ],
        onFeature = onFeature,
        isEnabled = isEnabled
      )

      Toolbar.ToolbarButtonGroup(
        [ List; Link; Quote; Image ],
        onFeature = onFeature,
        isEnabled = isEnabled
      )

      Toolbar.ToolbarButtonGroup(
        [ Code; CodeBlock ],
        onFeature = onFeature,
        isEnabled = isEnabled
      )
    }
