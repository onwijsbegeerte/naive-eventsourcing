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

type RetrieveBalance =
    { AccountId : AccountId }

type Query = RetrieveBalance of RetrieveBalance

// let validateMoneyDepositedCommand md =
//     if md.DepositAmount > 0m then Success md
//     else WorkFlowError (ValidationError "money deposite must be positive")

// let validateMoneyWithdrawCommand mw =
//     if mw.WithdrawAmount > 0m then Success mw
//     else WorkFlowError (ValidationError "money deposite must be positive")

// let validateMoneyWithdrawCommand mw =
//     if mw.WithdrawAmount > 0m then Success mw
//     else WorkFlowError (ValidationError "money deposite must be positive")

// let validateOpenAccount oa =
//     let inner acc =
//     if oa.Name <> String.Empty then Success oa
//     else WorkFlowError (ValidationError "money deposite must be positive")


// type OpenAccount =
//     { Name : string
//       Location : string
//       Birthday : DateTime }

// let Validate command =
//     match command with
//         | MoneyDeposited md -> validateMoneyDepositedCommand md
//         | MoneyWithdraw mw -> validateMoneyWithdrawCommand mw
//         | OpenAccount  oa ->

// let stringNotEmpty s propertyName = if s = "" then Error (sprintf "%s must have a value" propertyName) else Ok s

// let NameNotEmpty i = 
//     stringNotEmpty i.Name "Name"
    

// let NameMustBeLessThen100 i = if i.Name.Length > 100 then Error "Name must be less then 100 characters" else Ok i


//"hello" |> StringNotEmpty

// let Validate (event : DepositEventDto) rules : Result<DepositEvent,Error> =
//     List.fold    
