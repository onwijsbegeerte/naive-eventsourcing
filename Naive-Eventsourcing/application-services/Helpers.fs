module applicationServices.Helpers
open NaiveEventsouring.Domain.Events

let getAmounts event =
    match event with
    | MoneyWithdrawn w -> -w.WithdrawAmount
    | MoneyDeposited d -> d.DepositAmount

let sortByAccount accountId event =
    match event with
    | MoneyWithdrawn w when w.AccountId = accountId -> true
    | MoneyDeposited d when d.AccountId = accountId -> true
    | _ -> false

let getAccountId event =
    match event with
    | MoneyWithdrawn w -> w.AccountId
    | MoneyDeposited d -> d.AccountId

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
           | MoneyWithdrawn w when w.Date <= date -> true
           | MoneyDeposited d when d.Date <= date -> true
           | _ -> false)
    |> List.fold (fun state (event : Event) -> state + (getAmounts event)) 0m
