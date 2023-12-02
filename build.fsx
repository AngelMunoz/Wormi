#r "nuget: Fun.Build, 1.0.5"

open Fun.Result
open Fun.Build


pipeline "fable:watch" {
  description "Runs the Fable Compiler in watch mode"

  stage "fable" {
    run "dotnet fable src/Wormi.JS -o src/Wormi/wwwroot/fable --watch"
  // run (fun ctx -> async {
  //   return "dotnet fable src/Wormi.JS -o src/Wormi/wwwroot/fable --watch"
  // })
  }

  runIfOnlySpecified
}

pipeline "fable" {
  description "Runs the Fable Compiler"
  stage "fable" { run "dotnet fable src/Wormi.JS -o src/Wormi/wwwroot/fable" }
  runIfOnlySpecified
}

tryPrintPipelineCommandHelp ()
