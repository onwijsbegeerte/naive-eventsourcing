module NaiveEventsouring.Handler

open NaiveEventsouring.Domain
open NaiveEventsouring.Events
open NaiveEventsouring.Helpers

let StateChangeAgent = MailboxProcessor<AccountId>.Start(fun inbox ->
    let rec loop() = async {
        let! accountChanged = inbox.Receive()
        
        let events = SerialzeWorkflow.DeserializeWorkflow
        let result = getAmountFor accountChanged events
        
        printfn "%A change: new amount: %f " accountChanged result
        
        return! loop()
        }
    loop()
    )

let TransactionAgent = MailboxProcessor<Event>.Start(fun inbox ->

    // the message processing function
    let rec transactionLoop() = async {

        // read a message
        let! transactionEvent = inbox.Receive()

        SerialzeWorkflow.SerializeWorkflow transactionEvent

        // process a message
        printfn "transaction is: %A" transactionEvent
        
        StateChangeAgent.Post (getAccountId transactionEvent)
        // loop to top
        return! transactionLoop()
        }

    // start the loop
    transactionLoop()
    )