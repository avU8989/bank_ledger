using FluentValidation.Results;

namespace BankLedger.App.Common.Exceptions;

public class ValidationException : Exception
{

    public IDictionary<string, string[]> Errors { get; }
    public ValidationException() : base("One or more validation failures have occured.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        //group the failures by property name and create a dictionary of property name to corresponding error messages
        var failureGroups = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());

        Errors = failureGroups;
    }
}