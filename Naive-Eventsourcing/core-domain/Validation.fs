module NaiveEventsouring.Validation

type Test = {
    Name : string;
    Amount: decimal
}

let StringCannotBeEmpty<'a> (t : 'a) (selector : 'a -> string) =
    match selector t with
    | "" -> Error "string cannot be empty"
    | _ -> Ok t  

let DecimalMustBePositive<'a> (t : 'a) (selector : 'a -> decimal) =
    match selector t with
    | x when x <= 0m -> Error "Decimal needs To be Positive"
    | _ -> Ok t  

//let ValidateTeste (input : Test) =
//    let result1 = StringCannotBeEmpty input (fun i -> i.Name)
//    let result2 = DecimalMustBePositive input (fun i -> i.Amount)

    