module Wormi.JS.Browser

open Browser.WebStorage

[<RequireQualifiedAccess>]
module ExportedNames =

  [<Literal>]
  let ExtractFromStorage = "ExtractFromStorage"

  [<Literal>]
  let SaveToStorage = "SaveToStorage"

[<Literal>]
let private LocalDraftKey = "wormi-current"

[<CompiledName(ExportedNames.ExtractFromStorage)>]
let ExtractFromStorage () = localStorage.getItem LocalDraftKey


[<CompiledName(ExportedNames.SaveToStorage)>]
let SaveToStorage (value: string) =
  localStorage.setItem (LocalDraftKey, value)
