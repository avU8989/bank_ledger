namespace BankLedger.Infrastructure.Accounts;

using BankLedger.App.Ports;

public sealed class SimpleIbanGenerator : IIbanGenerator
{
    public string GenerateIban()
    {
        // Implement IBAN generation logic here
        // This is a placeholder implementation and should be replaced with actual logic
        return "DE89 3704 0044 0532 0130 00";
    }
}