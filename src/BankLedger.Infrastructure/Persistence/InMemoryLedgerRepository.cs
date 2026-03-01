//the implementation of our ILedgerRepository

//ok it is recommended before implementin the SQL i should implement an InMemory repo and test it accordingly 
//if everything passed i transition to a SQlLite Repo with persistence 

//quick note in C# you inherit and implement an interface by ":"
//sealed --> means that we prevent inherting --> therefore public sealed class x : y means x implements interface y 

//in repository it is recommended to usally not add business validation (business rules should be handled in the services that implements the use cases)
//but we SHOULD verify for data integrity id empty, entries to add if they are empty or null

using BankLedger.Core.Ledger;

public sealed class InMemoryLedgerRepository : ILedgerRepository
{
    private readonly List<LedgerEntry> _entries = new();

    public Task<IReadOnlyList<LedgerEntry>> GetEntriesAsync(string accountId)
    {
        if (string.IsNullOrWhiteSpace(accountId))
        {
            throw new ArgumentException("Account id is required", nameof(accountId));
        }

        var result = _entries.Where(e => e.AccountId == accountId)
            .OrderBy(e => e.TimeStamp)
            .ToList();

        return Task.FromResult((IReadOnlyList<LedgerEntry>)result);
    }

    public Task AddEntriesAsync(IEnumerable<LedgerEntry> entries)
    {
        if (entries is null)
        {
            throw new ArgumentNullException(nameof(entries));
        }
        _entries.AddRange(entries);
        return Task.CompletedTask;
    }
}