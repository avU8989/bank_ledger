using Microsoft.Extensions.DependencyInjection;

namespace BankLedger.App.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationDependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddScoped<AccountLookupService>();

        services.AddValidatorsFromAssemblyContaining(typeof(ApplicationDependencyInjection), ServiceLifetime.Transient);

    }
}