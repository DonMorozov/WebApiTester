namespace Morozov.WebApiTester

open System
open System
open System.IO

open Newtonsoft.Json

open Core
open TestScriptConfig
open TestScriptResult
open Tools

module Processor =           
    
    let ParseScriptConfig text =
        try
            Success <| JsonConvert.DeserializeObject<TestScriptConfig> text
        with ex -> CantExe <| sprintf "Can't parse test script file: %s" ex.Message
        
    let ResolveParams (unresolvedParams: Map<string, string>): Map<string, string> =
        unresolvedParams |> Map.map (fun k v -> if v = "~Guid" then Guid.NewGuid().ToString() else v)
        
    let StepFailureMessage (responseText: string) (expectedResposneText: string) : string =
        sprintf "Unexpected response \n \n Expected \n %s \n \n Received \n %s" expectedResposneText responseText 
        
    let TestStep (url: string) (requestText: string) (expectedResponseText: string): StepResult =
        let response = SendHttpPostRequest url "application/xml" requestText
        match response with
        | Error error -> CantExe error
        | Ok responseText ->
            if String.Equals(responseText, expectedResponseText) then Success "Ok"
            else Failure (StepFailureMessage responseText expectedResponseText)
    
    let SubstParams (templateText: string) (templateParams: Map<string, string>) =
        let SubstParam (acc:string) (paramName: string) (paramValue: string): string =
            acc.Replace("{" + paramName + "}", paramValue)
        templateParams |> Map.fold SubstParam templateText 
        
    let RunTestStep (templatePath: string) (testUrl:string) (resolvedTestParams: Map<string, string>)
        (accumulator: TestResult) (stepName: string) (stepConfig: StepConfig) : TestResult =
        let url = if String.IsNullOrWhiteSpace stepConfig.URL then testUrl else stepConfig.URL
        let templateParams = MergeMap resolvedTestParams (ResolveParams stepConfig.Params)        
        match accumulator with
        | CantExe results -> CantExe results
        | Failure results -> Failure results
        | Success results ->
            let request = ReadFile "request file" <| Path.Combine(templatePath,stepConfig.Request)
            match request with
            | Error error -> CantExe <| results.Add(stepName, CantExe error)
            | Ok requestTemplate ->
                let requestText = SubstParams requestTemplate templateParams
                let response = ReadFile "response file" <| Path.Combine(templatePath,stepConfig.Response)
                match response with
                | Error error -> CantExe <| results.Add(stepName, CantExe error)
                | Ok responseTemplate ->
                    let responseText = SubstParams responseTemplate templateParams
                    match TestStep url requestText responseText with
                    | CantExe result -> CantExe <| results.Add(stepName, CantExe result)
                    | Failure result -> Failure <| results.Add(stepName, Failure result)
                    | Success result -> Success <| results.Add(stepName, Success result)
                    
    let RunTest (templatePath: string)
        (accumulator: ScriptResult) (testName: string) (testConfig:TestConfig): ScriptResult =        
        let testParams = ResolveParams testConfig.Params
        let testResult: TestResult =
            testConfig.Steps
            |> Map.fold (RunTestStep templatePath testConfig.URL testParams) (Success Map.empty)
        (+=) accumulator testName testResult
        
    let RunScript script: ScriptResult =        
        script.Tests
        |> Map.fold (RunTest script.TemplatesPath) (Success Map.empty)
        
    let ToResult result =
        match result with
        | Success _ -> Success Map.empty
        | Failure _ -> Failure Map.empty
        | CantExe reason -> CantExe reason
    
    let RunTestScript (scriptFileName: string): ScriptResult =
        scriptFileName
        |> ReadFile "script file" |> ToExecutionResult
        |> !? ParseScriptConfig
        |> !? RunScript