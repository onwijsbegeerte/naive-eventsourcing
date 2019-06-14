module applicationServices.WriteRepository

open Core.ValueTypes
open System
open System.Threading.Tasks
open Microsoft.Azure.Documents.Client;

let uri = UriFactory.CreateDocumentCollectionUri("eventstoreDB", "events")

let createClient (endpoint : string) (key : string) : DocumentClient =
    new DocumentClient(Uri(endpoint), key)

let PersistEvent (client : DocumentClient) event : unit =
     client.CreateDocumentAsync(uri,event) |> ignore
    
let GetQuestion (client : DocumentClient) (questionId : QuestionId) =
    let mutable feedOptions = FeedOptions()
    feedOptions.MaxItemCount <- Nullable<int>(1)
    feedOptions.EnableCrossPartitionQuery <- true

    let events = client.CreateDocumentQuery<Core.Question.Event list>(uri, (sprintf "SELECT * FROM EVENTS WHERE QuestionId = '%s'" (questionId.ToString())) , feedOptions).GetEnumerator().Current
    
    printfn "%A" events
    
    List.fold(fun state event -> Core.Question.apply state event) (Core.Question.state.Create None) events
    