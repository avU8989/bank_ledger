namespace BankLedger.Core.Policies;

using BankLedger.Core.Accounts;

public interface IAccountPolicy
{
    bool CanCreateAccount(string accountName, AccountType accountType);
    bool CanCloseAccount(Account account);
    bool CanDeposit(Account account, decimal amount);
    bool CanWithdraw(Account account, decimal amount);
    bool CanTransfer(Account fromAccount, Account toAccount, decimal amount);

    decimal MonthlyFee(AccountType type);
    decimal? OverdraftLimit(AccountType type);
    decimal? InterestRate(AccountType type);
}