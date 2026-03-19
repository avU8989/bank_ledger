
using BankLedger.Core.Ledger;
using BankLedger.Core.Transactions;
namespace BankLedger.Core.Common.Factories;

public static class LedgerEntryFactory
{
    //static method to create Ledger Entries
    public static IReadOnlyList<LedgerEntry> FromTransaction(Core.Transactions.Transaction transaction)
    {
        switch (transaction.Type)
        {
            case TransactionType.Deposit:
                return new[]
                {
                    LedgerEntry.Create(transaction.ToAccountId!, +transaction.Amount, LedgerEntryType.Deposit, transaction)
                };
            case TransactionType.Withdrawal:
                return new[]
                {
                    LedgerEntry.Create(transaction.FromAccountId!, -transaction.Amount, LedgerEntryType.Withdrawal, transaction)
                };

            case TransactionType.Transfer:
                return new[]
                {
                  LedgerEntry.Create(transaction.FromAccountId!, -transaction.Amount, LedgerEntryType.TransferOut, transaction),
                  LedgerEntry.Create(transaction.ToAccountId!, +transaction.Amount, LedgerEntryType.TransferIn, transaction)
                };
            case TransactionType.Fee:
                return new[]
                {
                    LedgerEntry.Create(transaction.FromAccountId!, - transaction.Amount, LedgerEntryType.Fee, transaction)
                };
            default:
                throw new NotSupportedException($"Unsupported transaction type {transaction.Type}");
        }
    }
}