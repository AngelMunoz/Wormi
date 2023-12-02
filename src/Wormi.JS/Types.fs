namespace Wormi

[<Struct>]
type EditorTextFeatures =
  | Heading of size: int
  | Bold
  | Italic
  | Strikethrough
  | Highlight
  | Quote
  | CodeBlock of language: string ValueOption
  | HorizontalRule



[<AutoOpen>]
module Extensions =
  type EditorTextFeatures with

    member self.MdMarker =
      match self with
      | Bold -> "**"
      | Italic -> "*"
      | Strikethrough -> "~~"
      | Quote -> ">"
      | Highlight -> "`"
      | CodeBlock(ValueSome language) -> $"```{language}"
      | CodeBlock(ValueNone) -> "```"
      | HorizontalRule -> "---"
      | Heading size -> " ".PadLeft(size, '#')

[<RequireQualifiedAccess>]
module ModuleNames =
  [<Literal>]
  let Library = "/fable/Library.js"
