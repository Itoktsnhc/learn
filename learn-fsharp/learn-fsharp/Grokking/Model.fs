module Model

type UserId = int
type TransactionId = int

type CreditCard =
    { Number: string
      Expiry: string
      Cvv: string }

type User = 
    { Id: UserId
      CreditCard: CreditCard option
      Limit: double option}