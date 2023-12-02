module Wormi.JS.Library

open FsToolkit.ErrorHandling

open System
open Fable.Core
open Browser.Dom
open Browser.Types
#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif
open Wormi

[<RequireQualifiedAccess>]
module ExportedNames =

  [<Literal>]
  let ActOnTextArea = "ActOnTextArea"


let private applyAction (el: HTMLTextAreaElement, action: EditorTextFeatures) =
  let selStart = el.selectionStart |> Option.ofNull |> Option.defaultValue 0
  let selEnd = el.selectionEnd |> Option.ofNull |> Option.defaultValue 0

  match action with
  | Bold
  | Italic
  | Highlight
  | Strikethrough ->
    let oldText = if selEnd > selStart then el.value[selStart..selEnd] else ""

    $"{action.MdMarker}{oldText}{action.MdMarker}"
  | Heading(size) ->
    let headings = String.replicate size "#" + " "

    let oldText =
      if selEnd > selStart then
        let oldText = $"{el.value[selStart..selEnd]}".PadLeft(size, '#')
        el.value[selStart..selEnd]
      else
        ""

    $"\n{headings}{oldText}"
  | Quote ->
    let oldText =
      if selEnd > selStart then
        $"{el.value[selStart..selEnd]}"
      else
        ""

    $"\n{action.MdMarker} {oldText}"

  | CodeBlock(ValueSome language) ->
    let oldText =
      if selEnd > selStart then
        $"{el.value[selStart..selEnd]}"
      else
        ""

    $"\n{action.MdMarker}{language}\n{oldText}\n```"
  | CodeBlock(ValueNone) ->
    let oldText =
      if selEnd > selStart then
        $"{el.value[selStart..selEnd]}"
      else
        ""

    $"\n{action.MdMarker}\n{oldText}\n{action.MdMarker}"

  | HorizontalRule ->
    let oldText =
      if selEnd > selStart then
        $"{el.value[selStart..selEnd]}"
      else
        ""

    $"\n{action.MdMarker}\n{oldText}"

[<CompiledName(ExportedNames.ActOnTextArea)>]
let actOnTxtArea (selector: string, payload: string) =
  match Decode.fromString EditorTextFeatures.Decoder payload with
  | Ok action -> option {
      let! el =
        document.querySelector (selector)
        |> Option.ofNull
        |> Option.map (fun txt -> txt :?> HTMLTextAreaElement)

      let newText = applyAction (el, action)
      let selStart = el.selectionStart |> Option.ofNull |> Option.defaultValue 0
      let selEnd = el.selectionEnd |> Option.ofNull |> Option.defaultValue 0


      let newContent =
        el.value.Substring(0, selStart) + newText + el.value.Substring(selEnd)

      el.value <- newContent
      return newContent
    }
  | Error err -> None
