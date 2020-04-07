namespace Morozov.WebApiTester

open System

open System
open Core
open TestScriptResult
open Printer

module ResultInterpretator =
    let PrintStepResult stepName (result: StepResult) =
        printf "      "
        match result with
        | Success text -> Print3 stepName ConsoleColor.Green " [Success] " text
        | CantExe text -> Print3 stepName ConsoleColor.Yellow " [CantExe] " text
        | Failure text -> Print3 stepName ConsoleColor.Red " [Failure] " text
    
    let PrintTestResult testName (result: TestResult) =
        printfn ""
        let PrintDetails (result: StepsResult) =             
            result |> Map.iter PrintStepResult
        match result with
        | Success stepsResult ->
            Print3 "   *** " ConsoleColor.Green (sprintf "%s [Success]" testName) " ***"
            PrintDetails stepsResult
        | CantExe stepsResult ->
            Print3 "   *** " ConsoleColor.Yellow (sprintf "%s [CantExe]" testName) " ***"
            PrintDetails stepsResult
        | Failure stepsResult ->
            Print3 "   *** " ConsoleColor.Red (sprintf "%s [Failure]" testName) " ***"
            PrintDetails stepsResult
    
    let InterpretResult (result: ScriptResult)=        
        let PrintDetails (result: TestsResult) =             
            result |> Map.iter PrintTestResult
            
        match result with
        | Success testsResult ->
            Print3 "********* " ConsoleColor.Green "Success" " *********"
            PrintDetails testsResult
            0
        | Failure testsResult ->
            Print3 "********* " ConsoleColor.DarkRed "Failure" " *********"
            PrintDetails testsResult
            -1
        | CantExe reason ->
            Print3 "********* " ConsoleColor.DarkYellow (sprintf "Can't execute because [%s]!" reason) " *********"
            -2            