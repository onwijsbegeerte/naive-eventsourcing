module applicationServices.Handler

open System
open NaiveEventsouring.Domain.Commands
open applicationServices
open NaiveEventsouring.Domain.Events
open NaiveEventsouring.Domain.Queries
open NaiveEventsouring.Domain.Account

let CommandHandler = 
    MailboxProcessor<Command>.Start(fun inbox ->
        let rec listen() =
            async {
                let! command = inbox.Receive()
                
                match command with
                    | DepositMoney md ->  CompositionRoot.SaveEvent (MoneyDeposited {EventId = Guid.NewGuid(); Version = 1; AccountId = (AccountId md.AccountId); Date = DateTime.Now; DepositAmount = md.DepositAmount})
                    
                    | WithdrawMoney mw ->  CompositionRoot.SaveEvent (MoneyWithdrawn {EventId = Guid.NewGuid(); Version = 1; AccountId = (AccountId mw.AccountId); Date = DateTime.Now; WithdrawAmount = mw.WithdrawAmount})
                    
                    | OpenAccount  oa -> failwith "not implemented"

                return! listen()
            }

        listen())

//
//let QueryHandler =
//     MailboxProcessor<Query>.Start(fun inbox ->
//        let rec listen() =
//            async {
//                let! query = inbox.Receive()
//      
//                let events = CompositionRoot.RetrieveEvents
//                match query with
//                    | AccountBalance ab ->  return! Helpers.getAmountFor ab.AccountId events
//                    
//                    | AllAccounts aa ->  CompositionRoot.SaveEvent (MoneyWithdrawn {EventId = Guid.NewGuid(); Version = 1; AccountId = (AccountId mw.AccountId); Date = DateTime.Now; WithdrawAmount = mw.WithdrawAmount})
//                    
//                    | TotalBalanceOfAllAccounts  tba -> failwith "not implemented"
//
//                return! listen()
//            }
//
//        listen())