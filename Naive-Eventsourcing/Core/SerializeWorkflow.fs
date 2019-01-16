module NaiveEventsouring.SerializeWorkflow

open System
open NaiveEventsouring.Events
open NaiveEventsouring.Domain
open Newtonsoft.Json
open System.IO

type JsonString = JsonString of string

let unwrapJson (JsonString json) = json

let MapToDepositEvent(d : DepositEventDto) : DepositEvent =
    { EventId = Guid.Parse(d.EventId)
      Version = d.Version
      AccountId = AccountId d.AccountId
      Date = (DateTime.Parse d.Date)
      DepositAmount = d.DepositAmount }

let MapToWithdrawEvent(w : WithdrawEventDto) : WithdrawEvent =
    { EventId = Guid.Parse(w.EventId)
      Version = w.Version
      AccountId = AccountId w.AccountId
      Date = (DateTime.Parse w.Date)
      WithdrawAmount = w.WithdrawAmount }

let MapToDepositDto(d : DepositEvent) : DepositEventDto =
    { EventId = d.EventId.ToString()
      Version = d.Version
      AccountId = (unwrap d.AccountId)
      Date = d.Date.ToString()
      DepositAmount = d.DepositAmount }

let MapToWithdrowDto(w : WithdrawEvent) : WithdrawEventDto =
    { EventId = w.EventId.ToString()
      Version = w.Version
      AccountId = (unwrap w.AccountId)
      Date = w.Date.ToString()
      WithdrawAmount = w.WithdrawAmount }

let MapToDomain(eventDto : EventDto) : Event =
    match eventDto with
    | DepositEvent d -> Deposit(MapToDepositEvent d)
    | WithdrawEvent w -> Withdraw(MapToWithdrawEvent w)

let MapToDto(event : Event) : EventDto =
    match (event : Event) with
    | Withdraw w -> WithdrawEvent(MapToWithdrowDto w)
    | Deposit d -> DepositEvent(MapToDepositDto d)

let ConvertToJsonString(eventDto : EventDto) : JsonString =
    ((JsonString(JsonConvert.SerializeObject eventDto)))

let FileName event =
    match (event : Event) with
    | Withdraw w -> w.EventId.ToString()
    | Deposit d -> d.EventId.ToString()

let SerializeToDomainObject(eventsOnDisk : JsonString list) : EventDto list =
    eventsOnDisk
    |> List.map unwrapJson
    |> List.map JsonConvert.DeserializeObject<EventDto>

let DeserializeWorkflow(events : unit -> JsonString list) : Event list =
    events()
    |> SerializeToDomainObject
    |> List.map MapToDomain

let SerializeWorkflow (persist : Event -> JsonString -> unit) (event : Event) =
    event
    |> MapToDto
    |> ConvertToJsonString
    |> persist event
