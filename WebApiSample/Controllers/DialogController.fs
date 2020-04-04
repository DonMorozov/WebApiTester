namespace Morozov.WebApiSample.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Morozov.WebApiSample.Data.DialogData
open Morozov.WebApiSample.Processors

[<ApiController>]
[<Route("[controller]")>]
type SampleController(logger: ILogger<SampleController>) =
    inherit ControllerBase()
    
    [<HttpGet>]
    [<Route("api")>]
    member __.Get() =                     
        ObjectResult("Only POST-queries are allowed")

    [<HttpPost>]
    [<Route("api")>]
    member __.Post([<FromBody>] request: Request) =
        let response: Response = DialogProcessor.Process request             
        ObjectResult(response)
