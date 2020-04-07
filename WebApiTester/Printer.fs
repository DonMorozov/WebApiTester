namespace Morozov.WebApiTester

open System

module Printer =    
    let Print color str =
        let currentColor = Console.ForegroundColor
        Console.ForegroundColor <- color
        printf "%s" str
        Console.ForegroundColor <- currentColor

    let PrintLn color str =
        let currentColor = Console.ForegroundColor
        Console.ForegroundColor <- color
        printfn "%s" str
        Console.ForegroundColor <- currentColor
        
    let Print3 prefix color str postfix =
        let currentColor = Console.ForegroundColor
        printf "%s" prefix
        Console.ForegroundColor <- color
        printf "%s" str
        Console.ForegroundColor <- currentColor
        printfn "%s" postfix