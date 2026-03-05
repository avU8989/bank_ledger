namespace BankLedger.Infrastructure.Persistence;

using BankLedger.Core.Accounts;
using BankLedger.App.Ports;

public sealed class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<Guid, Account> _accounts = new();

    public Task AddAsync(Account account)
    {
        _accounts[account.Id] = account;
        return Task.CompletedTask;
    }

    public Task<Account> GetByIdAsync(string id)
    {
        _accounts.TryGetValue(Guid.Parse(id), out var account);
        return Task.FromResult(account);
    }

    public Task UpdateAsync(Account account)
    {
        if (!_accounts.ContainsKey(account.Id))
        {
            throw new KeyNotFoundException($"Account with id {account.Id} not found");
        }
        _accounts[account.Id] = account;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string accountId)
    {
        if (!_accounts.ContainsKey(Guid.Parse(accountId)))
        {
            throw new KeyNotFoundException($"Account with id {accountId} not found");
        }
        _accounts.Remove(Guid.Parse(accountId));
        return Task.CompletedTask;
    }

}