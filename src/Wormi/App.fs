namespace Wormi

open Microsoft.AspNetCore.Components
open Fun.Blazor

type MainLayout() as this =
  inherit LayoutComponentBase()

  let content = div {
    class' "wormi-app"
    this.Body
  }

  override _.BuildRenderTree(builder) =
    content.Invoke(this, builder, 0) |> ignore


type Routes() =
  inherit FunComponent()

  override _.Render() = Router'() {
    AppAssembly(typeof<Routes>.Assembly)

    Found(fun routeData -> RouteView'() {
      RouteData routeData

      DefaultLayout typeof<MainLayout>
    })

    NotFound(
      section {
        h1 { "Not found" }
        p { "Sorry, there's nothing at this address." }
      }
    )
  }
