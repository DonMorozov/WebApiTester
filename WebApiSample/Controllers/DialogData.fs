namespace Morozov.WebApiSample.Data

open System
open System.Runtime.Serialization

module DialogData =
    [<CLIMutable>]
    [<DataContract>]
    [<Struct>]
    type HelloRequest =
        {
          [<DataMember>] Name: string
          [<DataMember>] Greeting: string      
          [<DataMember>] DialogID: string
        }

    [<CLIMutable>]
    [<DataContract>]
    [<Struct>]
    type HelloResponse =
        {
          [<DataMember>] DialogID: string
          [<DataMember>] Proceed: string
        }
        
    [<CLIMutable>]
    [<DataContract>]
    [<Struct>]
    type DialogRequest =
        {
          [<DataMember>] DialogID: string
          [<DataMember>] Answer: string
        }

    [<CLIMutable>]
    [<DataContract>]
    [<Struct>]
    type DialogResponse =
        {
          [<DataMember>] DialogID: string
          [<DataMember>] Question: string
        }
        
    [<CLIMutable>]
    [<DataContract>]
    [<Struct>]
    type Request =
        {
          [<DataMember(EmitDefaultValue = false)>] HelloRequest: Nullable<HelloRequest>
          [<DataMember(EmitDefaultValue = false)>] DialogRequest: Nullable<DialogRequest>
        }
        
    [<CLIMutable>]
    [<DataContract>]
    [<Struct>]
    type Response =
        {
          [<DataMember(EmitDefaultValue = false)>] HelloResponse: Nullable<HelloResponse>
          [<DataMember(EmitDefaultValue = false)>] DialogResponse: Nullable<DialogResponse>
        }