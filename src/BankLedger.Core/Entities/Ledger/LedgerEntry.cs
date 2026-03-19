namespace BankLedger.Core.Ledger;

using BankLedger.Core.Transactions;

//should be the hardfact 
public sealed record LedgerEntry
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTimeOffset TimeStamp { get; init; } = DateTimeOffset.UtcNow;

    public required string AccountId { get; init; }

    public required decimal Amount { get; init; }

    public required LedgerEntryType Type { get; init; }

    //linking back to the transaction
    public required Guid TransactionId { get; init; }

    public string Description { get; init; } = "";

    private LedgerEntry() { }

    public static LedgerEntry Create(string accountId, decimal amount, LedgerEntryType type, Transaction transaction)
    {
        return new LedgerEntry
        {
            AccountId = accountId,
            Amount = amount,
            Type = type,
            TransactionId = transaction.Id,
            Description = transaction.Description
        };
    }
}

