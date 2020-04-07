namespace Morozov.WebApiTester

open Core
open TestScriptResult
open Processor
open ResultInterpretator

module Main =

    let ReadFileNameFromArgv(argv: string []): ExecutionResult<string, string, TestsResult> =
        if argv.Length <> 1
        then CantExe <| sprintf "Expected the only (instead %d) argument - service tester script file name" argv.Length
        else Success argv.[0]

    [<EntryPoint>]
    let main argv =
        ReadFileNameFromArgv argv
        |> !? RunTestScript
        |> InterpretResult