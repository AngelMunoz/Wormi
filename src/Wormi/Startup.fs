#nowarn "0020"

open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Wormi
open Wormi.Services
open Wormi.Services.Markdown
open Wormi.Services.TextArea
open Wormi.Services.LocalStorage

let builder =
  WebAssemblyHostBuilder.CreateDefault(Environment.GetCommandLineArgs())

builder.RootComponents.Add<Routes>("#app")

builder.Services
  .AddSingleton<ITextAreaService>(TextArea.factory)
  .AddSingleton<ILocalStorageService>(LocalStorage.factory)
  .AddSingleton<IMarkdownRenderer>(fun _ -> Markdown.factory ())

builder.Services.AddFunBlazorWasm()

builder.Build().RunAsync()
