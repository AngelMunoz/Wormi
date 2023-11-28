namespace Wormi.Components

open Fun.Blazor
open Microsoft.AspNetCore.Components.Routing

type Navbar =

  static member inline Create() = nav {
    NavLink'() {
      Match NavLinkMatch.All
      href "/about"
      "About"
    }
  }
