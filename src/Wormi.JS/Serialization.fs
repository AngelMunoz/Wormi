namespace Wormi

#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

[<AutoOpen>]
module Serialization =

  type EditorTextFeatures with

    static member Encoder: Encoder<EditorTextFeatures> =
      Encode.Auto.generateEncoderCached (CaseStrategy.PascalCase)

    static member Decoder: Decoder<EditorTextFeatures> =
      Decode.Auto.generateDecoderCached (CaseStrategy.PascalCase)
