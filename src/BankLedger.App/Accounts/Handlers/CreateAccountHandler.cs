namespace BankLedger.App.Accounts.Handlers.CreateAccount;

using System.Threading;
using BankLedger.App.Accounts.Commands.CreateAccount;
using BankLedger.App.Ports;
using BankLedger.Core.Accounts;
using BankLedger.Core.Common.Factories;

using MediatR;

//by using MediatR, we can decouple the command handling logic from the rest of the application making it easier to maintain and test
//think of mediatR as a middleman/router between the endpoint and the business logic (handler that knows how to process the command and return the result)
//the http request comes in --> ASP.NET binds JSON to the command object (creates command)-> endpoint calls sender.Send(command) - its like the telling 
//"hey mediatr, take this request and find the right handler for it and execute the logic in that handler and return the result back to me" --> mediatR 
//finds the handler for the command (CreateAccountHandler) and executes the Handle method --> result goes back to the endpoint and then the endpoint returns the response to the client

//using command + handler -- is only there when i want to model one specific action (CRUD) that has specific input and output
//use a service when we have more complex busines logic, the logic is shared/reusable across multiple actions, or when we want to encapsulate a specific domain concept
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

    public async Task<CreateAccountResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
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
        await _repo.AddAsync(account);

        //6. Return the account id or the account itself (depending on the use case)
        return new CreateAccountResult(account.Id);
    }
}