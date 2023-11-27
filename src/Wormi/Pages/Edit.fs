namespace Wormi.Pages

open System
open Microsoft.AspNetCore.Components
open Fun.Blazor

[<Route "/edit/{PostId?}">]
type Editor() =
  inherit FunBlazorComponent()

  [<Parameter>]
  member val PostId = String.Empty with get, set


  override self.Render() = article { h1 { $"Editor: %s{self.PostId}" } }
