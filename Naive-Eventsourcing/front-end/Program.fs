module FrontEnd.App

open FSharp.Control.Tasks
open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe

// ---------------------------------
// Views
// ---------------------------------

module Views =
    open GiraffeViewEngine

    let layout (content: XmlNode list) =
        html [] [
            head [] [
                title []  [ encodedText "Api" ]
                link [ _rel  "stylesheet"
                       _type "text/css"
                       _href "/main.css" ]
            ]
            body [] content
        ]

    let partial () =
        h1 [] [ encodedText "Api" ]
   
open Microsoft.AspNetCore.Http

// ---------------------------------
// Web app
// ---------------------------------

let warblerA f a = f a a

let submitEvent : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let! command = ctx.BindJsonAsync<NaiveEventsouring.Domain.Commands.Command>()
            applicationServices.Handler.CommandHandler.Post command
            
            return! Successful.OK command next ctx
        }

let retrieveEvents (next: HttpFunc) (ctx : HttpContext) =
    let events = applicationServices.CompositionRoot.RetrieveEvents()
    json events next ctx

//let getTotalFor (accountId : int) (next : HttpFunc) (ctx : HttpContext) =
//    
//    let! result = Handler.QueryHandler.PostAndAsyncReply<NaiveEventsouring.Queries.AllAccounts>()
//
//    
//    let amountFor = getAmountFor (AccountId accountId) (CompositionRoot.RetrieveEvents())
//    json amountFor next ctx

let webApp =
    choose [
        GET >=>
            choose [
               route "/transaction" >=> retrieveEvents
//               routef "/transaction/%i" getTotalFor
            ]
        POST >=>
            choose [
                route "/transaction" >=> submitEvent
            ]
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
    (match env.IsDevelopment() with
    | true  -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Error)
           .AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")
    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(contentRoot)
        .UseIISIntegration()
        .UseWebRoot(webRoot)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0