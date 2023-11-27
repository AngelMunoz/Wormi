#nowarn "0020"

open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Fun.Blazor
open Wormi

let builder =
  WebAssemblyHostBuilder.CreateDefault(Environment.GetCommandLineArgs())

builder.RootComponents.Add<Routes>("#app")

builder.Services.AddFunBlazorWasm()

builder.Build().RunAsync()
