namespace Core
    module ValueTypes =
        open System

        type QuestionId = QuestionId of Guid
        type AnwserId = AnswerId of Guid
        type Body = Body of string
        type PointId = PointId of Guid
        type AccountId = AccountId of Guid
        type Rate = | Plus | Min
        type PointReciever = | QuestionId | AnwserId

        type Point = {
          PointReciever : PointReciever
          PointId : PointId
          Method : Rate
        }

        type Tag = Tag of string

    [<RequireQualifiedAccess>]
    module Anwser =
        open System
        open ValueTypes

         type State = {
          AnwserId : AnwserId
          AccountId : AccountId
          QuestionId : QuestionId
          Created : DateTime
          Body : Body
          Points : Point list
        }

    [<RequireQualifiedAccess>]
    module Question =
        open System
        open ValueTypes

        type AskQuestion = {
            AccountId : AccountId
            Question : string
            Body : string
            Tags : Tag list
        }

        type AnwserQuestion = {
            AccountId : AccountId
            QuestionId : QuestionId
            Anwser : string
            Body : string
        }

        type GivePoint = {
            AccountId : AccountId
            PointReciever : PointReciever
            Method : Rate
        }

        type QuestionAsked = {
            AccountId : AccountId
            Question : string
            Body : string
            Tags : Tag list
        }

        type QuestionAnwsered = {
            AccountId : AccountId
            QuestionId : QuestionId
            Body : string
        }

        type state = {
           QuestionId : QuestionId
           AccountId : AccountId
           Created : DateTime
           Title : string
           Body : Body
           Points : Point list
           Anwsers : Anwser.State list
           Tags : Tag list
           Active : bool
        }
        with static member Create(accountId) = {
                 QuestionId = QuestionId.QuestionId(Guid.NewGuid());
                 AccountId = match accountId with Some a -> a | _ -> AccountId.AccountId(Guid.NewGuid()); 
                 Created = DateTime.Now;
                 Title = String.Empty;
                 Body = Body String.Empty;
                 Points = [];
                 Anwsers = [];
                 Tags = [];
                 Active = false;
                 }

        let private applyQuestionAsked (item : state) (qa : QuestionAsked) =
           { item with
                state.Body = Body qa.Body;
                state.AccountId = qa.AccountId;
                state.Title = qa.Question;
                state.Created = DateTime.Now;
                state.QuestionId = QuestionId.QuestionId(Guid.NewGuid());
                state.Points = [];
                state.Tags = qa.Tags
                state.Anwsers = [];
                state.Active = true }

        let private applyAnwsers (item : state) (qaw : QuestionAnwsered) =
            let anwser : Anwser.State = { AnwserId = AnswerId(Guid.NewGuid());
                                    AccountId = qaw.AccountId;
                                    Body = Body qaw.Body;
                                    Points = [];
                                    QuestionId = qaw.QuestionId;
                                    Created = DateTime.Now; }
            { item with Anwsers = (item.Anwsers @ [ anwser ]) }
 
        type Event =
            | QuestionAsked of QuestionAsked
            | QuestionAnwsered of QuestionAnwsered

        let private createAskQuestionEvent (aq : AskQuestion) =
                Event.QuestionAsked { AccountId = aq.AccountId;
                                     Question = aq.Question;
                                     Body = aq.Body;
                                     Tags = aq.Tags }

        let private createAnwserQuestionEvent (anq : AnwserQuestion) =
                Event.QuestionAnwsered { AccountId = anq.AccountId;
                                           QuestionId = anq.QuestionId;
                                           Body = anq.Body }

        type Command =
            | AskQuestion of AskQuestion
            | AnwserQuestion of AnwserQuestion
 //            | GivePoint of GivePoint

        let apply (item : state) = function
            | QuestionAsked qa -> applyQuestionAsked item qa
            | QuestionAnwsered qaw -> applyAnwsers item qaw

        let exec item =
            let apply event =
                let newItem = apply item event
                event
            function
            | AskQuestion(aq) -> (createAskQuestionEvent aq) |> apply
            | AnwserQuestion(anq) -> (createAnwserQuestionEvent anq) |> apply