namespace Persistence

open NPoco
open System.Data.SQLite
open NaiveEventsouring.Events
open System.IO
open NaiveEventsouring.SerializeWorkflow

module Entities =
    
    [<CLIMutable>]
    type Event = {
        Version : int;
        EventType: string;
        Payload: string
    } 

module EventsAccess =
    let private connString = "Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), "EventStore.db")

    //let mapToDbEntity (event : Event) =
    //    let eventType = 
    //        match event with
    //            |  Withdraw w -> string Event.Withdraw
    //            |  Deposit d -> string Event.Deposit
    //    let entity : Entities.Event = {Version = 1; EventType = eventType; Payload = event}
    //    entity

    let AddEvent (event : Event) (eventString : JsonString) =
        use conn = new SQLiteConnection(connString)
        conn.Open()

        use txn: SQLiteTransaction = conn.BeginTransaction()
        let cmd = conn.CreateCommand()
        cmd.Transaction <- txn
        cmd.CommandText <- @"insert into Event(Version, EventType, Payload) values ($Version, $EventType, $Payload)"

        let eventType = 
            match event with
                   |  Withdraw w -> string Event.Withdraw
                   |  Deposit d -> string Event.Deposit

        let unwrap = function JsonString x -> x

        cmd.Parameters.AddWithValue("$Version", 1) |> ignore
        cmd.Parameters.AddWithValue("EventType", eventType ) |> ignore
        cmd.Parameters.AddWithValue("Payload", unwrap eventString) |> ignore
        
        cmd.ExecuteNonQuery() |> ignore
        txn.Commit()


    let getEvents() =
        use conn = new SQLiteConnection(connString)
        conn.Open()

        let query = "select * from Event"
        use db = new Database(conn)
        db.Fetch<Entities.Event>(query) |> Seq.map(fun x -> JsonString x.Payload) |> Seq.toList
