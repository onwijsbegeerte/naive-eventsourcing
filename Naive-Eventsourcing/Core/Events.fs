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
    { AccountId : AccountId
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
