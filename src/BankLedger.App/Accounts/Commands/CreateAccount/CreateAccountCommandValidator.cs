using System.Data;
using FluentValidation;

namespace BankLedger.App.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{

    private static readonly HashSet<string> ValidCurrencyCodes = new HashSet<string>
    {
        "USD", "EUR", "GBP", "JPY", "CHF", "CAD", "AUD", "NZD", "CNY", "SEK",
    };
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Account name is required.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid account type.");

        //if is empty stop and dont look for the other rules for currency --> cascading
        RuleFor(x => x.Currency)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Currency should not be empty.")
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.")
            .Must(BeAValidCurrency).WithMessage("Invalid currency type.");
    }

    private bool BeAValidCurrency(string arg)
    {
        return ValidCurrencyCodes.Contains(arg.ToUpper());
    }
}