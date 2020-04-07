namespace Morozov.WebApiTester

open System
open System.IO
open System.Net
open System.Net.Http
open System.Security
open System.Text

open Core

module Tools =
    let ReadFile fileType fileName : Result<string, string> =
        try
            Ok <| File.ReadAllText fileName
        with
        | :? ArgumentNullException -> Error <| sprintf "The %s file name is empty" fileType
        | :? ArgumentException -> Error <| sprintf "The %s file name '%s' contains incorrect chars" fileType fileName
        | :? PathTooLongException -> Error <| sprintf "The %s file path too long" fileType
        | :? DirectoryNotFoundException -> Error <| sprintf "Directory of the %s file '%s' not found" fileType fileName
        | :? FileNotFoundException -> Error <| sprintf "The %s file '%s' not found" fileType fileName
        | :? IOException -> Error <| sprintf "Error reading %s file '%s'" fileType fileName
        | :? UnauthorizedAccessException -> Error <| sprintf "Access to %s file '%s' is not allowed" fileType fileName
        | :? SecurityException -> Error <| sprintf "Access to %s file '%s' is not permitted" fileType fileName
        | :? NotSupportedException -> Error <| sprintf "The %s file format is not supported" fileType
        
    let SendHttpPostRequest (url: string) (requestType: string) (requestText: string): Result<string, string> =
        try            
            let task = (new HttpClient()).PostAsync(url, new StringContent(requestText, Encoding.UTF8, requestType))            
            task.Wait()
            let response = task.Result
            let responseText =
                if response.StatusCode <> HttpStatusCode.OK then
                    response.StatusCode.ToString()
                else
                    let task = response.Content.ReadAsStringAsync()
                    task.Wait()
                    task.Result
            Ok responseText
        with
            | ex -> Error <| sprintf "Http Error: %s" ex.Message