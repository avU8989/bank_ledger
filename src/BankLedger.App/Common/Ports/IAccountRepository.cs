namespace BankLedger.App.Ports;

using BankLedger.Core.Accounts;
public interface IAccountRepository
{
    Task<Account> GetByIdAsync(Guid accountId);
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(string accountId);
}