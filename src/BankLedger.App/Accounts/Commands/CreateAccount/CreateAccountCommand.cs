namespace BankLedger.App.Accounts.Commands.CreateAccount;

using MediatR;
using BankLedger.Core.Accounts;

public sealed class CreateAccountCommand : IRequest<CreateAccountResult>
{
    public required string Name { get; init; }
    public required AccountType Type { get; init; }
    public decimal? InitialBalance { get; init; }
    public string Currency { get; init; } = "EUR";
}