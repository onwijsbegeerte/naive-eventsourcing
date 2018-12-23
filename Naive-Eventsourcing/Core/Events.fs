module NaiveEventsouring.Events

open NaiveEventsouring.Domain
open System

type WithdrawEvent =
    { EventId : Guid
      Version : int
      AccountId : AccountId
      Date : DateTime
      WithdrawAmount : decimal }

type DepositEvent =
    { EventId : Guid
      Version : int
      AccountId : AccountId
      Date : DateTime
      DepositAmount : decimal }

type DepositEventDto =
    { EventId : string
      Version : int
      AccountId : int
      Date : string
      DepositAmount : decimal }

type WithdrawEventDto =
    { EventId : string
      Version : int
      AccountId : int
      Date : string
      WithdrawAmount : decimal }

type Event =
    | Withdraw of WithdrawEvent
    | Deposit of DepositEvent

type EventDto =
    | DepositEvent of DepositEventDto
    | WithdrawEvent of WithdrawEventDto

type MoneyDeposited =
    { AccountId : AccountId
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

type RetrieveBalance =
    { AccountId : AccountId }

type Query = RetrieveBalance of RetrieveBalance

let stringNotEmpty s propertyName = if s = "" then Error (sprintf "%s must have a value" propertyName) else Ok s

let NameNotEmpty i = if i.Name = "" then Error "Name must have a value" else Ok i 
let NameMustBeLessThen100 i = if i.Name.Length > 100 then Error "Name must be less then 100 characters" else Ok i


//"hello" |> StringNotEmpty

// let Validate (event : DepositEventDto) rules : Result<DepositEvent,Error> =
//     List.fold    
