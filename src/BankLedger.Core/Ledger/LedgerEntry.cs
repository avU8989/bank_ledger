namespace BankLedger.Core.Ledger;


public enum EntryType
{
    Deposit,
    Withdrawal,
    TransferIn,
    TransferOut,
    Fee,
    OverdraftFee
}

//should be the hardfact 
public sealed record LedgerEntry
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTimeOffset TimeStamp { get; init; } = DateTimeOffset.UtcNow;

    public required string AccountId { get; init; }

    public required decimal Amount { get; init; }

    public required EntryType Type { get; init; }

    //linking back to the transaction
    public required Guid TransactionId { get; init; }

    public string Description { get; init; } = "";
}