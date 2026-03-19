using BankLedger.App.Ports;
using BankLedger.Core.Common.Factories;
using BankLedger.Core.Ledger;
using BankLedger.Core.Transactions;

//ApplyTransactionHandler is the application service that handles the ApplyTransactionCommand
//it is responsible for validating the command, creating the transaction and applying it to the ledger

//We could later refactor in each Withdrawal, Deposit, Transfer and Fee handlers, but for now we will keep it simple and have a single handler for all transaction types
public sealed class ApplyTransactionHandler
{
    private readonly ILedgerRepository _repo;

    public ApplyTransactionHandler(ILedgerRepository repo)
    {
        _repo = repo;
    }

    public async Task<ApplyTransactionResult> HandleAsync(ApplyTransactionCommand cmd)
    {

        //does the account exist
        //TODO - we can add an AccountService that checks if the account exists, but for now we will just assume that the account exists and let the repository throw an exception if it doesn't

        //map domain to transactionm (domain enforces the rules, so we can be sure that the transaction is valid)
        var transaction = cmd.Type switch
        {
            TransactionType.Deposit => Transaction.Deposit(toAccountId: cmd.ToAccountId, amount: cmd.Amount, description: cmd.Description, at: cmd.TimeStamp),
            TransactionType.Withdrawal => Transaction.Withdrawal(fromAccountId: cmd.FromAccountId, amount: cmd.Amount, description: cmd.Description, at: cmd.TimeStamp),
            TransactionType.Transfer => Transaction.Transfer(fromAccountId: cmd.FromAccountId, toAccountId: cmd.ToAccountId!, amount: cmd.Amount, description: cmd.Description, at: cmd.TimeStamp),
            TransactionType.Fee => Transaction.Fee(fromAccountId: cmd.FromAccountId, amount: cmd.Amount, description: cmd.Description, at: cmd.TimeStamp),
            _ => throw new ArgumentOutOfRangeException(nameof(cmd.Type), "Unknown transaction type")
        };

        //check if it needs a debit
        if (NeedsDebit(transaction))
        {
            var balance = await GetBalanceAsync(transaction.FromAccountId!);

            //Insufficient funds case
            if ((balance - transaction.Amount) < 0m)
            {
                throw new InvalidOperationException("Insufficient funds!");
            }
        }

        //create the Ledger Entry
        IReadOnlyList<LedgerEntry> ledgerEntries = LedgerEntryFactory.FromTransaction(transaction);

        //add the ledger entries to the repo
        //for now only inmemory
        await _repo.AddEntriesAsync(ledgerEntries);

        return new ApplyTransactionResult(transaction.Id);
    }


    private async Task<decimal> GetBalanceAsync(string accountId)
    {
        var entries = await _repo.GetEntriesAsync(accountId);
        return entries.Sum(e => e.Amount);
    }

    private bool NeedsDebit(Transaction transaction)
        => transaction.Type is TransactionType.Withdrawal or TransactionType.Transfer or TransactionType.Fee;

}