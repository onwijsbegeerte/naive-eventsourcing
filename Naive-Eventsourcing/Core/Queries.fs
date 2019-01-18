module NaiveEventsouring.Queries

type AccountBalance = {AccountId : int}

type AllAccounts = AllAccounts

type TotalBalanceOfAllAccounts = TotalBalanceOfAllAccounts

type Query =    
    | AccountBalance of AccountBalance
    | AllAccounts of AllAccounts
    | TotalBalanceOfAllAccounts of TotalBalanceOfAllAccounts