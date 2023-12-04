namespace Wormi.Components

open Fun.Blazor
open Fun.Css
open Microsoft.AspNetCore.Components.Routing

module NavbarStyles =
  let navbar = ruleset ".wo-navbar ul" {
    displayFlex
    justifyContentSpaceAround
    alignItemsCenter
    listStyleTypeNone
    fontSize "1.2rem"
    flexWrapWrap
  }

  let navbarItem = ruleset ".wo-navbar .wo-navbar-item" {
    padding "0.5rem"

  }

type Navbar =

  static member inline Create() = nav {
    class' "wo-navbar"
    style { fontFamily "sans-serif" }

    ul {

      li {
        class' "wo-navbar-item"

        NavLink'() {
          Match NavLinkMatch.All
          href "/about"
          "About"
        }
      }

      li {
        class' "wo-navbar-item"

        NavLink'() {
          Match NavLinkMatch.All
          href "/"
          "Let's start"
        }
      }

      li {
        class' "wo-navbar-item"

        NavLink'() {
          Match NavLinkMatch.Prefix
          href "/edit"
          "New Post"
        }
      }
    }

    styleElt {
      NavbarStyles.navbar
      NavbarStyles.navbarItem
    }
  }
