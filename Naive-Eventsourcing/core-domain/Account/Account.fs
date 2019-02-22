 namespace NaiveEventsouring.Domain
     module Account =
        type AccountId = AccountId of int
        
        type AccountStatus =
            | Negative
            | Inactive
            | Active
        
        type Account =
            {
              AccountId : AccountId
              Name : string
              CurrentBalance : decimal
              AccountStatus : AccountStatus }
        
        let unwrap (AccountId id) = id
        
        
     module Commands =
            open System
            type DepositMoney =
                { AccountId : int
                  DepositAmount : decimal }
            
            type WithdrawMoney =
                { AccountId : int
                  WithdrawAmount : decimal }
            
            type OpenAccount =
                { Name : string
                  Location : string
                  Birthday : DateTime }
            
            type Command =
                | DepositMoney of DepositMoney
                | WithdrawMoney of WithdrawMoney
                | OpenAccount of OpenAccount
                    
      
        module Events =
            open System
            open Account
            
            type MoneyWithdrawn =
                { EventId : Guid
                  Version : int
                  AccountId : AccountId
                  Date : DateTime
                  WithdrawAmount : decimal }
            
            type MoneyDeposited =
                { EventId : Guid
                  Version : int
                  AccountId : AccountId
                  Date : DateTime
                  DepositAmount : decimal }
            
            
            type Event =
                | MoneyWithdrawn of MoneyWithdrawn
                | MoneyDeposited of MoneyDeposited

        module Queries =
            type AccountBalance = {AccountId : int}
    
            type AllAccounts = AllAccounts
            
            type TotalBalanceOfAllAccounts = TotalBalanceOfAllAccounts
            
            type Query =    
                | AccountBalance of AccountBalance
                | AllAccounts of AllAccounts
                | TotalBalanceOfAllAccounts of TotalBalanceOfAllAccounts


            type Replies =
                | Balance of AsyncReplyChannel<decimal>
                | Accounts of AsyncReplyChannel<int>
                | TotalBalance of AsyncReplyChannel<int>





// type WorkFlowError =
//     | ValidationError of string
//     | SerializationError of string
//     | PercistenceError of string

// type Result<'Entity> =
//     | Success of 'Entity
//     | WorkFlowError of WorkFlowError
    
// let Bind f =
//     fun twoTrackInput ->
//         match twoTrackInput with
//         | Success s -> f s
//         | WorkFlowError e -> WorkFlowError e
