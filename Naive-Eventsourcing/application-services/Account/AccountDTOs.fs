module applicationServices.Account.AccountDTOs

     type JsonString = JsonString of string

     let unwrapJson (JsonString json) = json

     type DepositEventDto =
                { EventId : string
                  Version : int
                  AccountId : int
                  Date : string
                  DepositAmount : decimal }

      type WithdrawEventDto =
                { EventId : string
                  Version : int
                  AccountId : int
                  Date : string
                  WithdrawAmount : decimal }


      type EventDto =
                | DepositEvent of DepositEventDto
                | WithdrawEvent of WithdrawEventDto