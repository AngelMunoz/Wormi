namespace Wormi.Components

open System
open Microsoft.AspNetCore.Components
open FSharp.Data.Adaptive
open Fun.Blazor

open Wormi

type Toolbar =
  static member ToolbarButton
    (
      feature: EditorTextFeatures,
      ?onFeature: EditorTextFeatures -> unit,
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
        | Quote -> "“”"
        | HorizontalRule -> "HR"
        | CodeBlock(language) -> ""
        | Heading(size) -> ""
      }

      title
    }

  static member ToolbarButtonGroup
    (
      buttons: EditorTextFeatures list,
      ?onFeature: EditorTextFeatures -> unit,
      ?isEnabled: EditorTextFeatures -> bool
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
      ?onFeature: EditorTextFeatures -> unit,
      ?isEnabled: EditorTextFeatures -> bool
    ) =
    let onFeature = defaultArg onFeature ignore
    let isEnabled = defaultArg isEnabled (fun _ -> true)

    section {
      class' "editor-toolbar"

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
        [ Quote; Highlight; HorizontalRule ],
        onFeature = onFeature,
        isEnabled = isEnabled
      )
    }
