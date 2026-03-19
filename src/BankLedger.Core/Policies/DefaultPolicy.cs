namespace BankLedger.Core.Policies;

using BankLedger.Core.Accounts;

//Later we can split into separate policies for different concerns (e.g. account creation, transactions, fees, etc.)
public sealed class DefaultPolicy : IAccountPolicy

{
    public bool CanCreateAccount(string accountName, AccountType accountType)
    {
        // For simplicity, we allow all account creations in the default policy.
        return true;
    }
    public bool CanCloseAccount(Account account)
    {
        // For simplicity, we allow closing any account in the default policy.
        return true;
    }
    public bool CanDeposit(Account account, decimal amount)
    {
        // For simplicity, we allow deposits to any account in the default policy.
        return true;
    }
    public bool CanWithdraw(Account account, decimal amount)
    {
        // For simplicity, we allow withdrawals from any account in the default policy.
        return true;
    }
    public bool CanTransfer(Account fromAccount, Account toAccount, decimal amount)
    {
        // For simplicity, we allow transfers between any accounts in the default policy.
        return true;
    }
    public decimal MonthlyFee(AccountType type)
    {
        // Define monthly fees based on account type
        return type switch
        {
            AccountType.Giro => 5m,
            AccountType.Student => 0m,
            AccountType.Business => 10m,
            AccountType.Savings => 2m,
            AccountType.Credit => 15m,
            _ => 5m
        };
    }
    public decimal? OverdraftLimit(AccountType type)
    {
        return type switch
        {
            AccountType.Giro => 500m,
            AccountType.Student => 0m,
            AccountType.Business => null, // No overdraft limit for business accounts
            AccountType.Savings => null, // No overdraft limit for savings accounts
            AccountType.Credit => null, // No overdraft limit for credit accounts
            _ => 500m
        };
    }
    public decimal? InterestRate(AccountType type)
    {
        return type switch
        {
            AccountType.Giro => 0m,
            AccountType.Student => 0m,
            AccountType.Business => 0m,
            AccountType.Savings => 0.01m, // 1% interest for savings accounts
            AccountType.Credit => -0.05m, // -5% interest for credit accounts (i.e. they pay interest)
            _ => 0m
        };
    }

}