module applicationServices.QuestionHandler

    let handleAskQuestion (aq : Core.Question.AskQuestion) persistEvent : unit =
        let init = Core.Question.state.Create(Option.Some(aq.AccountId))
        let event = Core.Question.exec init (Core.Question.Command.AskQuestion aq)
        persistEvent event |> ignore
        
    let handleAnwserQuestion (awq : Core.Question.AnwserQuestion) persistEvent getQuestion  : unit =
        let question = getQuestion (awq.QuestionId.ToString()) 
        let event = Core.Question.exec question (Core.Question.Command.AnwserQuestion awq)
        persistEvent event |> ignore
    
    let CommandHandler persistEvent getQuestion = 
        MailboxProcessor<Core.Question.Command>.Start(fun inbox ->
            let rec listen() =
                async {
                    let! command = inbox.Receive()
                    match command with
                    | Core.Question.AskQuestion aq -> handleAskQuestion aq persistEvent
                    | Core.Question.AnwserQuestion awq -> handleAnwserQuestion awq persistEvent getQuestion
                    return! listen()
                }
            listen())