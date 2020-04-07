namespace Morozov.WebApiTester

open System

open Newtonsoft.Json

module TestScriptConfig =
    
    type StepConfig =
        {
            [<JsonProperty(Required = Required.Default)>]
            URL: string
            
            [<JsonProperty(Required = Required.Always)>]
            Request: string
            
            [<JsonProperty(Required = Required.Always)>]
            Response: string
            
            [<JsonProperty(Required = Required.Default)>]
            Params: Map<string, string>
        }

    type TestConfig =
        {
            [<JsonProperty(Required = Required.Default)>]
            URL: string
            
            [<JsonProperty(Required = Required.Default)>]
            Params: Map<string, string>
            
            [<JsonProperty(Required = Required.Always)>]
            Steps: Map<string, StepConfig>
        }
    
    type TestScriptConfig =
        {
            [<JsonProperty(Required = Required.Always)>]
            TemplatesPath: string
            
            [<JsonProperty(Required = Required.Always)>]            
            Tests: Map<string, TestConfig>   
        }
        
    type ExecutionConfig =
        {
            TemplatePath: string
            URL: string
            Params: Map<string, string>
        }    