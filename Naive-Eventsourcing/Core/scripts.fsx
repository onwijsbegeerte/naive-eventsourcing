open System.Windows.Forms
//TransactionAgent.Post(Withdraw {EventId = (Guid.NewGuid()); Version = 1; AccountId = (AccountId 4); Date = DateTime.Now.AddDays(-10.0); WithdrawAmount = 100m })



// TransactionAgent.Start()
// let depositEvent : DepositEvent  = {EventId = (Guid.NewGuid()); Version = 1; AccountId = (AccountId 4); Date = DateTime.Now.AddDays(-10.0); DepositAmount = 500m } 
// TransactionAgent.Post (Deposit depositEvent)

// //let event1 = Withdraw { Version = 1; AccountId = (AccountId 4); Date = DateTime.Now.AddDays(-7.0); WithdrawAmount = 10m }
// //let event2 = Withdraw { Version = 1; AccountId = (AccountId 4); Date = DateTime.Now.AddDays(-6.0); WithdrawAmount = 50m }
// //let event3 = Withdraw { Version = 1; AccountId = (AccountId 4); Date = DateTime.Now.AddDays(-3.0); WithdrawAmount = 30m }
// //let event4 = Withdraw { Version = 1; AccountId = (AccountId 5); Date = DateTime.Now.AddDays(-1.0); WithdrawAmount = 20m }
// //let event5 = Withdraw { Version = 1; AccountId = (AccountId 5); Date = DateTime.Now.AddDays(-4.0); WithdrawAmount = 20m }
// //let event6 = Withdraw { Version = 1; AccountId = (AccountId 4); Date = DateTime.Now.AddDays(-10.0); WithdrawAmount = 20m }
// //let events : Event list = [event1 ; event2 ; event3 ; event4 ; event6 ; event5 ; depositEvent]
// //
// //
// //

//let depositEvent : DepositEvent  = {EventId = (Guid.NewGuid()); Version = 1; AccountId = (AccountId 4); Date = DateTime.Now.AddDays(-10.0); DepositAmount = 500m } 
//    // let withdrawEvent : WithdrawEvent  = {EventId = (Guid.NewGuid()); Version = 1; AccountId = (AccountId 4); Date = DateTime.Now.AddDays(-10.0); WithdrawAmount = 100m } 

//    //  SerializeWorkflow (Withdraw withdrawEvent)
//    let results = Helpers.getAmountFor (AccountId 4) (SerializeWorkflow.DeserializeWorkflow)
    
//    printfn "results: %A" results
    
//    // Handler.TransactionAgent.Post (Deposit depositEvent)
//    // Handler.TransactionAgent.Post (Withdraw {EventId = (Guid.NewGuid()); Version = 1; AccountId = (AccountId 4); Date = DateTime.Now.AddDays(-10.0); WithdrawAmount = 100m } )
//    //  Handler.TransactionAgent.Post (Withdraw {EventId = (Guid.NewGuid()); Version = 1; AccountId = (AccountId 4); Date = DateTime.Now.AddDays(-10.0); WithdrawAmount = 100m } )
// //getAmountFor (AccountId 4) events
// //
// //getAmountForTime (AccountId 4) (DateTime.Now.AddDays(-10.0)) events
