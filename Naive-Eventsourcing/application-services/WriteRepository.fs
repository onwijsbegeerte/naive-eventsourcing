module applicationServices.WriteRepository

open System
open Microsoft.Azure.Documents.Client;

let uri = UriFactory.CreateDocumentCollectionUri("eventstoreDB", "events")

let createClient (endpoint : string) (key : string) : DocumentClient =
    new DocumentClient(Uri(endpoint), key)

let PersistEvent event (client : DocumentClient) =
     client.CreateDocumentAsync(uri,event)
    
let GetQuestion (client : DocumentClient) questionId =
    let mutable feedOptions = FeedOptions()
    feedOptions.MaxItemCount <- Nullable<int>(1)
    feedOptions.EnableCrossPartitionQuery <- true

    let events = client.CreateDocumentQuery<Core.Question.Event list>(uri, (sprintf "SELECT * FROM EVENTS WHERE QuestionId = '%s'" questionId) , feedOptions).GetEnumerator().Current
    List.fold(fun state event -> Core.Question.apply state event) (Core.Question.state.Create None) events
    