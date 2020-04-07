namespace Morozov.WebApiTester

module Core =
    type ExecutionResult<'Success, 'CantExe,'Failure> =
        | Success of 'Success
        | CantExe of 'CantExe
        | Failure of 'Failure    
    
    let (!?) nextProcessor result =
        match result with
        | Failure details -> Failure details
        | CantExe details -> CantExe details
        | Success details -> nextProcessor details
        
    let MergeMap (map1:Map<'a,'b>) (map2:Map<'a,'b>) = 
        Map(Seq.concat [(Map.toSeq map1); (Map.toSeq map2)])    