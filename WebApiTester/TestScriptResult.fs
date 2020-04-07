namespace Morozov.WebApiTester

open Core

module TestScriptResult =
    type StepResult =
        ExecutionResult<string, string, string>
    
    type StepsResult =
        Map<string, StepResult>
    
    type TestResult =
        ExecutionResult<StepsResult, StepsResult, StepsResult>
        
    type TestsResult =
        Map<string, TestResult>
    
    type ScriptResult =
        ExecutionResult<TestsResult, string, TestsResult>
        
    let (+=) (scriptResult: ScriptResult) (testName: string) (testResult: TestResult): ScriptResult  =
        match scriptResult with
        | CantExe reason -> CantExe reason
        | Failure testsResult -> Failure <| testsResult.Add(testName, testResult)
        | Success testsResult ->            
            match testResult with
            | Success _ -> Success <| testsResult.Add(testName, testResult)
            | CantExe _ -> Failure <| testsResult.Add(testName, testResult)
            | Failure _ -> Failure <| testsResult.Add(testName, testResult)                
            
    let ToExecutionResult (result: Result<string, string>) : ExecutionResult<string, string, TestsResult> =
        match result with
        | Ok details -> Success details
        | Error details -> CantExe details