module NaiveEventsouring.Helpers

open NaiveEventsouring.Events

let getAmounts event =
    match event with
    | Withdraw w -> -w.WithdrawAmount
    | Deposit d -> d.DepositAmount

let sortByAccount accountId event =
    match event with
    | Withdraw w when w.AccountId = accountId -> true
    | Deposit d when d.AccountId = accountId -> true
    | _ -> false

let getAccountId event =
    match event with
    | Withdraw w -> w.AccountId
    | Deposit d -> d.AccountId

let sortByDate accountId events = List.sortBy (fun e -> e.Date) events

let getAmountFor accountId (events : Event list) =
    events
    |> List.filter (fun event -> sortByAccount accountId event)
    |> List.fold (fun state (event : Event) -> state + (getAmounts event)) 0m

let getAmountForTime accountId date (events : Event list) =
    events
    |> List.filter (fun event -> sortByAccount accountId event)
    |> List.filter (fun event -> 
           match event with
           | Withdraw w when w.Date <= date -> true
           | Deposit d when d.Date <= date -> true
           | _ -> false)
    |> List.fold (fun state (event : Event) -> state + (getAmounts event)) 0m
