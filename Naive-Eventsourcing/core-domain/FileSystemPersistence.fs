module NaiveEventsouring.FileSystemPersistence
//
//open System.IO
//open System
//open NaiveEventsouring.SerializeWorkflow
//
//let SaveToDisk (name : string) (json : JsonString) : unit =
//    let basePath =
//        sprintf "%s/events/%s" (Directory.GetCurrentDirectory())
//            (DateTime.Now.ToString("yyyy/MM/dd"))
//    Directory.CreateDirectory basePath |> ignore
//    let file = sprintf "%s/Event-%s.json" basePath name
//    File.WriteAllText(file, unwrapJson (json))
//
//let RetrieveEvents() : JsonString list =
//    let eventRoot = sprintf "%s/events" (Directory.GetCurrentDirectory())
//    System.IO.Directory.GetFiles
//        (eventRoot, "*.json", SearchOption.AllDirectories)
//    |> Array.map System.IO.File.ReadAllText
//    |> Array.map JsonString
//    |> Array.toList
