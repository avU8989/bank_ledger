
namespace BankLedger.Web.DependencyInjection;

using BankLedger.Web.Infrastructure;
public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    }
}