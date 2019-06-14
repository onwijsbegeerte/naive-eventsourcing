module CompositionRoot
open Giraffe
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Http
open applicationServices
open Microsoft.Azure.Documents.Client;

let getClient (ctx: HttpContext) : DocumentClient =
    let settings = ctx.GetService<IConfiguration>()
    let endpoint = settings.["CosmosDB:Endpoint"]
    let key = settings.["CosmosDB:Key"]
    let client = WriteRepository.createClient endpoint key
    client
        
let GetQuestion client =
    WriteRepository.GetQuestion client
    
let PersistEvent client : Core.Question.Event -> unit =
    WriteRepository.PersistEvent client