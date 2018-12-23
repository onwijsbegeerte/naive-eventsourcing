module NaiveEventsouring.CompositRoot

let RetrieveEvents =
    NaiveEventsouring.SerializeWorkflow.DeserializeWorkflow
        NaiveEventsouring.FileSystemPersistence.RetrieveEvents
let SaveEvent =
    NaiveEventsouring.SerializeWorkflow.SerializeWorkflow
        NaiveEventsouring.FileSystemPersistence.SaveToDisk
