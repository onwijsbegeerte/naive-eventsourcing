module applicationServices.CompositionRoot
    open applicationServices.Account.SerializeWorkflows
    open applicationServices.Account.DataAccess
    
    let RetrieveEvents() =
        DeserializeWorkflow
            EventsAccess.getEvents
          
    let SaveEvent =
        SerializeWorkflow
            EventsAccess.AddEvent
