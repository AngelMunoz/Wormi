namespace Wormi.Pages

open System
open Microsoft.AspNetCore.Components
open Fun.Blazor

open Wormi.Components

[<Route "/edit/{PostId?}">]
type Editor() =
  inherit FunBlazorComponent()

  [<Parameter>]
  member val PostId = String.Empty with get, set


  override self.Render() = Editor.Editor "Hello World"
