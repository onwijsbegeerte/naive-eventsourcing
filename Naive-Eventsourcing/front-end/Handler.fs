module FrontEnd.Handler

open System
open NaiveEventsouring.Domain.Commands
open NaiveEventsouring.Domain.Events
open NaiveEventsouring.Domain.Account

let CommandHandler = 
    MailboxProcessor<Command>.Start(fun inbox ->
        let rec listen() =
            async {
                let! command = inbox.Receive()
                
                match command with
                    | DepositMoney md ->  FrontEnd.CompositionRoot.SaveEvent (MoneyDeposited {EventId = Guid.NewGuid(); Version = 1; AccountId = (AccountId md.AccountId); Date = DateTime.Now; DepositAmount = md.DepositAmount})
                    
                    | WithdrawMoney mw ->  FrontEnd.CompositionRoot.SaveEvent (MoneyWithdrawn {EventId = Guid.NewGuid(); Version = 1; AccountId = (AccountId mw.AccountId); Date = DateTime.Now; WithdrawAmount = mw.WithdrawAmount})
                    
                    | OpenAccount  oa -> failwith "not implemented"

                return! listen()
            }

        listen())

