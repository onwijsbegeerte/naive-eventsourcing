module Api.CompositionRoot

let RetrieveEvents =
    NaiveEventsouring.SerializeWorkflow.DeserializeWorkflow
        Persistence.EventsAccess.getEvents
        
let SaveEvent =
    NaiveEventsouring.SerializeWorkflow.SerializeWorkflow
        Persistence.EventsAccess.AddEvent
