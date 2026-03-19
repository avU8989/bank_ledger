using MediatR;
using ValidationException = BankLedger.App.Common.Exceptions.ValidationException;

//Validation behavior that runs before the request handler and checks for any validators for incoming requests. 
//If any validators are found, it executes them and gathers any validation failures. 
//If there are any validation failures, it throws a ValidationException 
//If validation passes, it continues to the next behavior or handler in the pipeline.
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            //Run all validators in parallel and gather results
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request, cancellationToken)));

            //Aggregate errors from all validators
            var failures = validationResults.SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count() > 0)
            {
                //If there are any validation failures, throw a ValidationException with the details
                throw new ValidationException(failures);
            }
        }

        //If validation passes, continue to the next behavior or handler in the pipeline
        return await next();
    }

}