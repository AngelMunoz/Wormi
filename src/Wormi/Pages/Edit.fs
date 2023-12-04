namespace Wormi.Pages

open System
open Microsoft.AspNetCore.Components
open Fun.Blazor

open Wormi
open Wormi.Components
open Microsoft.Extensions.Logging

[<Route "/edit/{PostId?}">]
type Editor() =
  inherit FunBlazorComponent()

  [<Parameter>]
  member val PostId = String.Empty with get, set

  [<Inject>]
  member val Logger = Unchecked.defaultof<ILogger<Editor>> with get, set

  override self.Render() = fragment { h1 { "Editor" } }
