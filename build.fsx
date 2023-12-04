#r "nuget: Fun.Build, 1.0.5"

open Fun.Build

module Stages =
  let Fable = stage "fable" {
    run "dotnet fable src/Wormi.JS -o src/Wormi/wwwroot/fable"
  }

  let Build = stage "build" { run "dotnet build src/Wormi" }

pipeline "watch" {
  description "Runs Fable + Wormi in watch mode"

  stage "run:watch" {
    paralle
    run "dotnet fable src/Wormi.JS -o src/Wormi/wwwroot/fable --watch"
    run "dotnet watch run --project src/Wormi"
  }

  runIfOnlySpecified
}

tryPrintPipelineCommandHelp ()
