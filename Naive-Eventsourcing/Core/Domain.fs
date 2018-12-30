module NaiveEventsouring.Domain

type AccountId = AccountId of int

type AccountStatus =
    | Negative
    | Inactive

type Account =
    { AccountId : AccountId
      Name : string
      CurrentBalance : decimal
      AccountStatus : AccountStatus }

let unwrap (AccountId id) = id

type WorkFlowError =
    | ValidationError of string
    | SerializationError of string
    | PercistenceError of string

type Result<'Entity> =
    | Success of 'Entity
    | WorkFlowError of WorkFlowError

let Bind f =
    fun twoTrackInput ->
        match twoTrackInput with
        | Success s -> f s
        | WorkFlowError e -> WorkFlowError e
