module applicationServices.Account.SerializeWorkflows

    open System
    open Newtonsoft.Json
    open applicationServices.Account.AccountDTOs
    open NaiveEventsouring.Domain.Account
    open NaiveEventsouring.Domain.Events
    open System.Globalization;
            
    let MapToDepositEvent(d : DepositEventDto) : MoneyDeposited =
        { EventId = Guid.Parse(d.EventId)
          Version = d.Version
          AccountId = AccountId d.AccountId
          Date = (DateTime.ParseExact(d.Date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))
          DepositAmount = d.DepositAmount }
    
    let MapToWithdrawEvent(w : WithdrawEventDto) : MoneyWithdrawn =
        { EventId = Guid.Parse(w.EventId)
          Version = w.Version
          AccountId = AccountId w.AccountId
          Date = (DateTime.ParseExact(w.Date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))
          WithdrawAmount = w.WithdrawAmount }
    
    let MapToDepositDto(d : MoneyDeposited) : DepositEventDto =
        { EventId = d.EventId.ToString()
          Version = d.Version
          AccountId = (unwrap d.AccountId)
          Date = d.Date.ToString()
          DepositAmount = d.DepositAmount }
    
    let MapToWithdrowDto(w : MoneyWithdrawn) : WithdrawEventDto =
        { EventId = w.EventId.ToString()
          Version = w.Version
          AccountId = (unwrap w.AccountId)
          Date = w.Date.ToString()
          WithdrawAmount = w.WithdrawAmount }
    
    let MapToDomain(eventDto : EventDto) : Event =
        match eventDto with
        | DepositEvent d -> MoneyDeposited(MapToDepositEvent d)
        | WithdrawEvent w -> MoneyWithdrawn(MapToWithdrawEvent w)
    
    let MapToDto(event : Event) : EventDto =
        match (event : Event) with
        | MoneyWithdrawn w -> WithdrawEvent(MapToWithdrowDto w)
        | MoneyDeposited d -> DepositEvent(MapToDepositDto d)
    
    let ConvertToJsonString(eventDto : EventDto) : JsonString =
        ((JsonString(JsonConvert.SerializeObject eventDto)))
    
    let FileName event =
        match (event : Event) with
        | MoneyWithdrawn w -> w.EventId.ToString()
        | MoneyDeposited d -> d.EventId.ToString()
    
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
