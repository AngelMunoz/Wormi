namespace Wormi

open Microsoft.AspNetCore.Components
open Fun.Blazor
open Microsoft.AspNetCore.Components.Routing

[<Route "/about">]
type About() =
  inherit FunBlazorComponent()

  override _.Render() = div {
    "about"
    br

    NavLink'() {
      Match NavLinkMatch.All
      href "/"
      "Home"
    }
  }
