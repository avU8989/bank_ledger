using BankLedger.App.Ports;
using BankLedger.Core.Accounts;

public sealed class AccountLookupService
{
    private readonly IAccountRepository _accountRepository;

    public AccountLookupService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Account> GetByIdOrThrowAsync(Guid accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);

        return account ?? throw new KeyNotFoundException($"Account with id {accountId} was not found.");
    }
}