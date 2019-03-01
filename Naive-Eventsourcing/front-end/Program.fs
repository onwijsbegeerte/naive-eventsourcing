module FrontEnd.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.Extensions.Configuration
// ---------------------------------
// Views
// ---------------------------------

module Views =
    open GiraffeViewEngine

    let layout (content : XmlNode list) =
        html [] [
            head [] [
                title [] [ encodedText "Api" ]
                link [ _rel "stylesheet"
                       _type "text/css"
                       _href "/main.css" ]
            ]
            body [] content
        ]

    let partial() =
        h1 [] [ encodedText "Api" ]

open CompositionRoot
open Core
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2
open RequestModels

// ---------------------------------
// Web app
// ---------------------------------

let warblerA f a = f a a

let AskQuestion =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
                let! request = ctx.BindJsonAsync<AskQuestionRequest>()
                let command : Question.AskQuestion = { AccountId = ValueTypes.AccountId(Guid.Parse(request.AccountId));
                                          Question = request.Title;
                                          Body = request.Body;
                                          Tags = (request.Tags |> List.map (fun t -> ValueTypes.Tag t)) }
                let getQuestion = CompositionRoot.GetQuestion (CompositionRoot.getClient ctx)
                let persistEvent = CompositionRoot.PersistEvent (CompositionRoot.getClient ctx)

//                let commandHandler = applicationServices.QuestionHandler.CommandHandler persistEvent getQuestion 
//                applicationServices.QuestionHandler.CommandHandler.Post (Question.Command.AskQuestion command) |> ignore
                return! Successful.OK request next ctx
        }
        
let GetQuestion (qid : string) (next : HttpFunc) (ctx : HttpContext) =
        json qid next ctx

let webApp =
    choose [
        GET >=>
            choose [
               routef "/question/%s" GetQuestion 
            ]
        POST >=>
            choose [
                route "/askQuestion" >=> AskQuestion
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
    | true -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Error)
           .AddConsole()
           .AddDebug() |> ignore

let configureAppConfiguration  (context: WebHostBuilderContext) (config: IConfigurationBuilder) =  
    config
        .AddJsonFile("appsettings.json",false,true)
        .AddJsonFile(sprintf "appsettings.%s.json" context.HostingEnvironment.EnvironmentName ,true)
        .AddEnvironmentVariables() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot = Path.Combine(contentRoot, "WebRoot")
    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(contentRoot)
        .UseIISIntegration()
        .UseWebRoot(webRoot)
        .ConfigureAppConfiguration(configureAppConfiguration)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0