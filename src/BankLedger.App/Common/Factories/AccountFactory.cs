namespace BankLedger.Core.Common.Factories;


using BankLedger.Core.Accounts;
using BankLedger.Core.Policies;

public sealed class AccountFactory
{
    private readonly IAccountPolicy _accountPolicy;

    public AccountFactory(IAccountPolicy accountPolicy)
    {
        _accountPolicy = accountPolicy;
    }

    public Account Create(string name, AccountType type, string iban, decimal initialBalance, string currency)
    {
        var monthlyFee = _accountPolicy.MonthlyFee(type);
        var overdraftLimit = _accountPolicy.OverdraftLimit(type);
        var interestRate = _accountPolicy.InterestRate(type);

        return Account.Create(
            name: name,
            type: type,
            balance: initialBalance,
            iban: iban,
            status: AccountStatus.Active,
            currency: currency,
            monthlyFee: monthlyFee,
            overdraftLimit: overdraftLimit,
            interestRate: interestRate
        );
    }
}