namespace Morozov.WebApiSample.Processors

open System
open Morozov.WebApiSample.Data.DialogData

module DialogProcessor =
    let ProcessHello (request: HelloRequest): HelloResponse =         
        if request.Name = "Mouse" then { DialogID = request.DialogID; Proceed = "Yes" }
        else { DialogID = request.DialogID; Proceed = "No" }
        
    let ProcessDialog (request: DialogRequest): DialogResponse =
        let ContinueDialog text =
            if  String.IsNullOrWhiteSpace text then
                "What cheese is the best?"
            else
                match text with
                | "Roquefort" -> "Rocky, my friend! How weather in Moscow?"
                | "Monterey Jack" -> "Monty, my friend! Where is Hackwrench?"
                | _ -> "Nice!"
        { DialogID = request.DialogID; Question = ContinueDialog request.Answer}
    
    let Process (request: Request): Response =
        if request.HelloRequest.HasValue && not request.DialogRequest.HasValue then
            {
                HelloResponse = System.Nullable(ProcessHello request.HelloRequest.Value)
                DialogResponse = System.Nullable()
            }
        elif not request.HelloRequest.HasValue && request.DialogRequest.HasValue then
            {
                HelloResponse = System.Nullable()
                DialogResponse = System.Nullable(ProcessDialog request.DialogRequest.Value)
            }
        else
            {
                HelloResponse = System.Nullable(); DialogResponse = System.Nullable()
            }
            