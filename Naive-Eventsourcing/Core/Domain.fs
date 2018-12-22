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
