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

  override self.Render() =
    let post = {
      _id = System.Guid.NewGuid().ToString()
      _rev = ValueNone
      title = ""
      content = ""
      status = PostStatus.InProgress
      metadata = ValueNone
      updated_at = ValueNone
      created_at = DateTime.Now
    }

    Editor.Editor(
      post,
      markAsDraft =
        (fun newContent ->
          self.Logger.LogInformation("Saving draft: {content}", newContent)),
      markDirty =
        (fun isDirty ->
          (self.Logger.LogInformation("Is dirty: {isDirty}", isDirty)))
    )
