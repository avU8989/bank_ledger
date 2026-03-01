
using BankLedger.Core.Ledger;
using BankLedger.Core.Transactions;
namespace BankLedger.Core.Common;

public static class LedgerEntryFactory
{
    //create one new Ledger Entry

    public static LedgerEntry NewEntry(string accountId, decimal amount, EntryType type, Transaction transaction) =>
        new()
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            Description = transaction.Description,
            TimeStamp = transaction.TimeStamp,
            AccountId = accountId,
            TransactionId = transaction.Id,
            Type = type,
        };


    //static method to create Ledger Entries
    public static IReadOnlyList<LedgerEntry> FromTransaction(Core.Transactions.Transaction transaction)
    {
        switch (transaction.Type)
        {
            case TransactionType.Deposit:
                return new[]
                {
                    NewEntry(transaction.ToAccountId!, +transaction.Amount, EntryType.Deposit, transaction)
                };
            case TransactionType.Withdrawal:
                return new[]
                {
                    NewEntry(transaction.FromAccountId!, -transaction.Amount, EntryType.Withdrawal, transaction)
                };

            case TransactionType.Transfer:
                return new[]
                {
                  NewEntry(transaction.FromAccountId!, -transaction.Amount, EntryType.TransferOut, transaction),
                  NewEntry(transaction.ToAccountId!, +transaction.Amount, EntryType.TransferIn, transaction)
                };
            case TransactionType.Fee:
                return new[]
                {
                    NewEntry(transaction.FromAccountId!, - transaction.Amount, EntryType.Fee, transaction)
                };
            default:
                throw new NotSupportedException($"Unsupported transaction type {transaction.Type}");
        }
    }
}