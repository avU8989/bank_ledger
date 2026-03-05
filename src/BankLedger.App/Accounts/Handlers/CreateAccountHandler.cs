namespace BankLedger.App.Accounts.Create;

using System.Threading;
using BankLedger.App.Ports;
using BankLedger.Core.Accounts;
using BankLedger.Core.Common.Factories;

using MediatR;

public sealed class CreateAccountHandler : IRequestHandler<CreateAccountCommand, CreateAccountResult>
{
    private readonly IAccountRepository _repo;
    private readonly IIbanGenerator _ibanGenerator;

    private readonly AccountFactory _accountFactory;


    public CreateAccountHandler(IAccountRepository repo, IIbanGenerator ibanGenerator, AccountFactory accountFactory)
    {
        _repo = repo;
        _ibanGenerator = ibanGenerator;
        _accountFactory = accountFactory;
    }

    public Task<CreateAccountResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {

        //Logic

        //1. Validate the command
        //TODO - later add validation middleware and move validation logic to a separate validator class
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException(nameof(request.Name), "Account name cannot be empty");
        }

        if (request.InitialBalance < 0m)
        {
            throw new ArgumentException(nameof(request.InitialBalance), "Initial balance cannot be negative");
        }

        //generate iban
        var iban = _ibanGenerator.GenerateIban();



        //2. Create the account
        var account = _accountFactory.Create(
            name: request.Name,
            type: request.Type,
            initialBalance: request.InitialBalance ?? 0m,
            iban: iban,
            currency: request.Currency
        );

        //3. Save the account to the repository

        //4. Create initial ledger entry for the initial balance
        //5. Save the ledger entry to the repository

        //6. Return the account id or the account itself (depending on the use case)
        return Task.FromResult(new CreateAccountResult(account.Id));
    }
}