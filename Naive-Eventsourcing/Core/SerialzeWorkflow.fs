module NaiveEventsouring.SerialzeWorkflow

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

let SaveToDisk (name : string) (json : JsonString) : unit =
    let basePath =
        sprintf "%s/events/%s" (Directory.GetCurrentDirectory()) 
            (DateTime.Now.ToString("yyyy/MM/dd"))
    Directory.CreateDirectory basePath |> ignore
    let file = sprintf "%s/Event-%s.json" basePath name
    File.WriteAllText(file, unwrapJson (json))

let FileName event =
    match (event : Event) with
    | Withdraw w -> w.EventId.ToString()
    | Deposit d -> d.EventId.ToString()

let SerializeWorkflow(event : Event) =
    event
    |> MapToDto
    |> ConvertToJsonString
    |> SaveToDisk(FileName event)

let RetrieveEvents : JsonString list =
    let eventRoot = sprintf "%s/events" (Directory.GetCurrentDirectory())
    System.IO.Directory.GetFiles
        (eventRoot, "*.json", SearchOption.AllDirectories)
    |> Array.map (fun fn -> fn)
    |> Array.map (fun f -> System.IO.File.ReadAllText(f))
    |> Array.map (fun f -> JsonString f)
    |> Array.toList

let SerializeToDomainObject(eventsOnDisk : JsonString list) : EventDto list =
    eventsOnDisk
    |> List.map (fun ejson -> unwrapJson ejson)
    |> List.map (fun edto -> JsonConvert.DeserializeObject<EventDto>(edto))

let DeserializeWorkflow : Event list =
    RetrieveEvents
    |> SerializeToDomainObject
    |> List.map (fun edto -> MapToDomain edto)
