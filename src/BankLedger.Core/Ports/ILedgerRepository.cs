//depends on the interface

//repository interfacee in thee application and the implementation in the infrastructure
//business code will depend on abstraction, not database files 


using BankLedger.Core.Ledger;

public interface ILedgerRepository
{
    //fetches ledger entries based on account id
    Task<IReadOnlyList<LedgerEntry>> GetEntriesAsync(string accountId);

    Task AddEntriesAsync(IEnumerable<LedgerEntry> entries);
}