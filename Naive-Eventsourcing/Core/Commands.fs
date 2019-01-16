module NaiveEventsouring.Commands
open System

type MoneyDeposited =
    { AccountId : int
      DepositAmount : decimal }

type MoneyWithdraw =
    { AccountId : int
      WithdrawAmount : decimal }

type OpenAccount =
    { Name : string
      Location : string
      Birthday : DateTime }

type Command =
    | MoneyDeposited of MoneyDeposited
    | MoneyWithdraw of MoneyWithdraw
    | OpenAccount of OpenAccount
