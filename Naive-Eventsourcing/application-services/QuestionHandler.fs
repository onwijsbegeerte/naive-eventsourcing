module applicationServices.QuestionHandler
    
//    let CreateQuestion accountId =
//        Core.Question.state.Create(accountId)
//    
//    let CreateQuestionEvent accountId initial =
//        let question : Core.Question.AskQuestion = {
//             AccountId = accountId;
//             Question = "is the earth Flat?";
//             Body = "I always wonderd...";
//             Tags = [ Core.ValueTypes.Tag "stupid question" ] }
//        
//        Core.Question.exec initial (Core.Question.Command.AskQuestion question)
//
//    let GetQuestion accountId =
//        let initial = Core.Question.state.Create(accountId)
//        let questionEvent = CreateQuestionEvent accountId initial
//        let stateAfterQuestion = Core.Question.apply initial questionEvent
//       
//        let awnser : Core.Question.AnwserQuestion = {
//             AccountId = accountId;
//             QuestionId = stateAfterQuestion.QuestionId;
//             Anwser = "No..."
//             Body = "Really it is not"
//            }
//        
//        let awnserEvent = Core.Question.exec stateAfterQuestion (Core.Question.Command.AnwserQuestion awnser)
//        Core.Question.apply stateAfterQuestion awnserEvent

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