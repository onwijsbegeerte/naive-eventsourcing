module NaiveEventsouring.Events

open NaiveEventsouring.Domain
open System

type WithdrawEvent = {
    EventId: Guid
    Version: int    
    AccountId: AccountId;
    Date: DateTime;
    WithdrawAmount: decimal;
}

type DepositEvent = {
    EventId: Guid
    Version: int
    AccountId:AccountId;
    Date: DateTime;
    DepositAmount: decimal;
}

type DepositEventDto = {
    EventId: string
    Version: int
    AccountId: int
    Date: string
    DepositAmount: decimal;
}

type WithdrawEventDto = {
    EventId: string
    Version: int    
    AccountId: int;
    Date: string;
    WithdrawAmount: decimal;
}

type Event =
    | Withdraw of WithdrawEvent
    | Deposit of DepositEvent


type EventDto =
    | DepositEvent of DepositEventDto
    | WithdrawEvent of WithdrawEventDto


