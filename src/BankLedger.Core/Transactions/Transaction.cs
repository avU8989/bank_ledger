namespace BankLedger.Core.Transactions;

//sealed --> no other class/record can inherit from it
//record --> only meant for data only objects
//basically immutable data object

//so in contrast to java c# expose the porperties (public), but you want to keep the setters private 
//so you dont get boilerplate code like getters to the property
//it will still prevent property = from outside 

//and public setters are generally bad for domain objects anyways
//you want it (a) to set it in a constructor once or (b) create a factory method, factory pattern, builder pattern or whatever to create the object
//audit - intent record that produces entries
public sealed record Transaction
{
    //GUID is basically just UUID
    //get; you can read the property 
    //init; you can set it only during object creation --> would be bad if uuid would be set later as they should be immutable
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTimeOffset TimeStamp { get; init; } = DateTimeOffset.UtcNow;

    //requires means that we have to set the property 
    //and properties use Pascal Case in C#
    public required TransactionType Type { get; init; }
    public required decimal Amount { get; init; } //should not be negative

    public string? FromAccountId { get; init; }
    public string? ToAccountId { get; init; }

    public string Description { get; init; } = "";

    //public named factories
    public static Transaction Deposit(string toAccountId, decimal amount, string? description = null, DateTimeOffset? at = null)
        => Create(type: TransactionType.Deposit, amount: amount, from: null, to: toAccountId, description: description, at: at);

    public static Transaction Withdrawal(string fromAccountId, decimal amount, string? description = null, DateTimeOffset? at = null)
        => Create(type: TransactionType.Withdrawal, amount: amount, from: fromAccountId, to: null, description: description, at: at);

    public static Transaction Transfer(string fromAccountId, string toAccountId, decimal amount, string? description = null, DateTimeOffset? at = null)
        => Create(type: TransactionType.Transfer, amount: amount, from: fromAccountId, to: toAccountId, description: description, at: at);

    public static Transaction Fee(string fromAccountId, decimal amount, string? description = null, DateTimeOffset? at = null)
        => Create(type: TransactionType.Fee, amount: amount, from: fromAccountId, to: null, description: description, at: at);

    private static Transaction Create(
        TransactionType type,
        decimal amount,
        string? from,
        string? to,
        string description,
        DateTimeOffset? at
    )
    {
        if (amount <= 0)
        {
            //why type nameof --> its better than typing "amount", because of typos and its refactor safe
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount should not be negative or 0");
        }

        //validation of TransactionTypes
        switch (type)
        {
            case TransactionType.Deposit:
                Require(to, "Deposit requires ToAccountId", nameof(to));
                Forbid(from, "Deposit must not have FromAccountId", nameof(from));
                break;
            case TransactionType.Withdrawal:
                Require(from, "Withdrawal requires FromAccountId", nameof(from));
                Forbid(to, "Withdrawal must not have ToAccountId", nameof(to));
                break;
            case TransactionType.Transfer:
                Require(from, "Transfer requires FromAccountId", nameof(from));
                Require(to, "Transfer requires ToAccountId", nameof(to));
                break;
            case TransactionType.Fee:
                Require(from, "Fee requires FromAccountId", nameof(from));
                Forbid(to, "Fee must not have ToAccountId", nameof(to));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), "Unknown transaction type");
        }

        return new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            Type = type,
            FromAccountId = from,
            ToAccountId = to,
            Description = description ?? "",
            TimeStamp = at ?? DateTimeOffset.UtcNow,
        };
    }

    private static void Require(string? value, string message, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(message, paramName);
    }

    private static void Forbid(string? value, string message, string paramName)
    {
        if (!string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(message, paramName);
    }


}