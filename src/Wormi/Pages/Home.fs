namespace Wormi.Pages


open Microsoft.AspNetCore.Components
open Microsoft.Extensions.Logging

open FSharp.Data.Adaptive
open Fun.Blazor
open Microsoft.AspNetCore.Components.Routing

[<Route "/">]
type Home() =
  inherit FunBlazorComponent()

  let onClick (logger: ILogger) current (update) _ =
    let newValue = current + 1
    logger.LogInformation("Clicked! {newValue}", newValue)
    update newValue

  override _.Render() =
    html.inject (fun (logger: ILogger<Home>) -> main {
      adaptiview () {
        let! counter, setCounter = cval(0).WithSetter()

        NavLink'() {
          Match NavLinkMatch.All
          href "/about"
          "About"
        }
        br
        NavLink'() {
          Match NavLinkMatch.All
          href "/edit"
          "New Post"
        }
        br
        NavLink'() {
          Match NavLinkMatch.All
          href "/edit/a123-s23"
          "Edit Post 'a123-s23'"
        }
        br

        button {
          onclick (onClick logger counter setCounter)
          $"Click me {counter}"
        }
      }
    })
