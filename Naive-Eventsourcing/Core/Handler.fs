module NaiveEventsouring.Handler

open System
open NaiveEventsouring.Domain
open NaiveEventsouring.Events
open NaiveEventsouring.Helpers
open NaiveEventsouring.Commands

let StateChangeAgent =
    MailboxProcessor<AccountId>.Start(fun inbox -> 
        let rec loop() =
            async { 
                let! accountChanged = inbox.Receive()
                let events = NaiveEventsouring.CompositionRoot.RetrieveEvents
                let result = getAmountFor accountChanged events
                printfn "%A change: new amount: %f " accountChanged result
                return! loop()
            }
        loop())

let TransactionAgent =
    MailboxProcessor<Event>.Start(fun inbox -> 
        // the message processing function
        let rec transactionLoop() =
            async { 
                // read a message
                let! transactionEvent = inbox.Receive()
                NaiveEventsouring.CompositionRoot.SaveEvent transactionEvent
                // process a message
                printfn "transaction is: %A" transactionEvent
                StateChangeAgent.Post(getAccountId transactionEvent)
                // loop to top
                return! transactionLoop()
            }
        // start the loop
        transactionLoop())

let CommandHandler = 
    MailboxProcessor<Command>.Start(fun inbox ->
        let rec listen() =
            async {
                let! command = inbox.Receive()
                
                match command with
                    | MoneyDeposited md -> NaiveEventsouring.CompositionRoot.SaveEvent (Deposit {EventId = Guid.NewGuid(); Version = 1; AccountId = (AccountId md.AccountId); Date = DateTime.Now; DepositAmount = md.DepositAmount})
                    
                    | MoneyWithdraw mw -> NaiveEventsouring.CompositionRoot.SaveEvent (Withdraw {EventId = Guid.NewGuid(); Version = 1; AccountId = (AccountId mw.AccountId); Date = DateTime.Now; WithdrawAmount = mw.WithdrawAmount})
                    
                    | OpenAccount  oa -> failwith "not implemented"

                return! listen()
            }

        listen())