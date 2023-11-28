#nowarn "0020"

open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Fun.Blazor
open Wormi
open Wormi.Services

let builder =
  WebAssemblyHostBuilder.CreateDefault(Environment.GetCommandLineArgs())

builder.RootComponents.Add<Routes>("#app")
builder.Services.AddSingleton<IDatabaseService, DatabaseService>()

builder.Services.AddFunBlazorWasm()

builder.Build().RunAsync()
