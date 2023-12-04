namespace Wormi

open Thoth.Json.Net

type LocalDraft = {
  title: string
  content: string
} with

  static member Encoder: Encoder<LocalDraft> =
    fun draft ->

      Encode.object [
        "title", Encode.string draft.title
        "content", Encode.string draft.content
      ]

  static member Decoder: Decoder<LocalDraft> =
    Decode.object (fun get -> {
      title = get.Required.Field "title" Decode.string
      content = get.Required.Field "content" Decode.string
    })
