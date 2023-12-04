namespace Wormi

open System


[<Struct>]
type PostMetadata = {
  id: string
  post_id: string
  tags: string list
  slug: string
}

[<Struct>]
type PostStatus =
  | Draft
  | InProgress
  | ReadyForReview
  | Finished

type Post = {
  _id: string
  _rev: string voption
  title: string
  content: string
  status: PostStatus
  metadata: PostMetadata voption
  updated_at: DateTime voption
  created_at: DateTime
}

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

  [<Literal>]
  let Browser = "/fable/Browser.js"
