module RequestModels

[<CLIMutable>]
type AskQuestionRequest = {
    AccountId : string
    Title: string
    Body: string
    Tags: string list
}

[<CLIMutable>]
type AnwserQuestionRequest = {
    AccountId : string
    QuestionId : string
    Body: string
    Tags: string list
}