using BankLedger.Core.Transactions;

public sealed record ApplyTransactionCommand(
    TransactionType Type,
    decimal Amount,
    string? FromAccountId,
    string? ToAccountId,
    string? Description,
    DateTime TimeStamp
);
