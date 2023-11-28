namespace Wormi.Pages


open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Components.Routing
open Microsoft.Extensions.Logging

open IcedTasks
open FSharp.Data.Adaptive
open Fun.Blazor


open Wormi.Components

[<Route "/">]
type Home() =
  inherit FunBlazorComponent()

  override _.Render() = article {
    Navbar.Create()

    main {
      section {
        h1 { "Review Current Work" }
      // Group by status
      }

      section {
        h3 { "Get back to work" }
      // draft list
      }
    }
  }
