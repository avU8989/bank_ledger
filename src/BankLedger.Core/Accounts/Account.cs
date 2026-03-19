namespace BankLedger.Core.Accounts;

using BankLedger.Core.Transactions;
public sealed class Account
{
    public Guid Id { get; }
    public string Name { get; }
    public AccountType Type { get; }
    public decimal Balance { get; private set; }
    public string Iban { get; }
    public AccountStatus Status { get; }
    public string Currency { get; } = "EUR";
    public decimal MonthlyFee { get; }
    public decimal? OverdraftLimit { get; }
    public decimal? InterestRate { get; }
    private Account(string name, AccountType type, decimal balance, string iban, AccountStatus status, string currency, decimal monthlyFee, decimal? overdraftLimit, decimal? interestRate)
    {
        Id = Guid.NewGuid();
        Name = name;
        Type = type;
        Balance = balance;
        Iban = iban;
        Status = status;
        Currency = currency;
        MonthlyFee = monthlyFee;
        OverdraftLimit = overdraftLimit;
        InterestRate = interestRate;
    }

    public static Account Create(string name, AccountType type, decimal balance, string iban, AccountStatus status, string currency, decimal monthlyFee, decimal? overdraftLimit, decimal? interestRate)
    {
        // Add any necessary validation logic here
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name), "Account name cannot be empty");
        }

        if (balance < 0m)
        {
            throw new ArgumentException(nameof(balance), "Initial balance cannot be negative");
        }

        if (string.IsNullOrWhiteSpace(iban))
        {
            throw new ArgumentException(nameof(iban), "IBAN cannot be empty");
        }

        return new Account(name, type, balance, iban, status, currency, monthlyFee, overdraftLimit, interestRate);
    }

    public void ApplyTransaction(Transaction transaction)
    {
        // Logic to apply a transaction to the account balance
        if (transaction.Type == TransactionType.Deposit)
        {
            Balance += transaction.Amount;
        }
        else if (transaction.Type == TransactionType.Withdrawal)
        {
            Balance -= transaction.Amount;
        }
    }

}