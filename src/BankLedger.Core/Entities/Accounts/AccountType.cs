namespace BankLedger.Core.Accounts;

public enum AccountType
{
    Giro, //default account type for private customers
    Student, //account type for students with special conditions (e.g. no fees, overdraft limit)
    Business, //account type for businesses with special conditions (e.g. higher fees, no overdraft limit)
    Savings, //account type for savings accounts with special conditions (e.g. higher interest rates, no overdraft limit)
    Credit, //account type for credit accounts with special conditions (e.g. higher fees, no overdraft limit)
}