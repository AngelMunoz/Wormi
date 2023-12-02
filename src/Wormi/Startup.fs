#nowarn "0020"

open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Wormi
open Wormi.Services
open Wormi.Services.Markdown

let builder =
  WebAssemblyHostBuilder.CreateDefault(Environment.GetCommandLineArgs())

builder.RootComponents.Add<Routes>("#app")

builder.Services
  .AddSingleton<IDatabaseService, DatabaseService>()
  .AddSingleton<ITextAreaService, TextAreaService>()
  .AddSingleton<IMarkdownRenderer>(fun _ -> Markdown.factory ())

builder.Services.AddFunBlazorWasm()

builder.Build().RunAsync()
