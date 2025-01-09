using Cine.Modules.Tickets.Application.ApiClients.Theater;
using Cine.Modules.Tickets.Application.Shows;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Database;
using Cine.Shared.Application.Queries;
using Cine.Shared.Application.Tasks;
using Cine.Shared.Application.Validation;
using Cine.Shared.Infrastructure.Events;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Refit;

namespace Cine.Modules.Tickets.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services,
        Action<ApplicationOptionsBuilder> builder)
    {
        var options = new ApplicationOptionsBuilder();
        builder(options);

        services.AddMediatR(c =>
            c.AddOpenBehavior(typeof(ValidationBehavior<,>))
                .RegisterServicesFromAssemblyContaining<IApplicationAssembly>());
        services.AddIntegrationEventHandlers();
        services.AddCommandHandlers();
        services.AddQueryHandlers();
        services.AddValidators();
        services.AddApiClients(options);

        services.AddSqlConnection(options.ConnectionString);

        return services;
    }

    public static IApplicationBuilder UseApplication(this IApplicationBuilder appBuilder)
    {
        appBuilder.UseIntegrationEvents();

        return appBuilder;
    }

    private static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
    {
        services.AddSingleton<ShowCreatedIntegrationEventHandler>();

        // return services
        // .Scan(scanner =>
        // {
        //     scanner.FromAssemblyOf<IApplicationAssembly>()
        //         .AddClasses(classes => classes.AssignableTo(typeof(IIntegrationEventHandler<>)))
        //         .AsImplementedInterfaces().WithSingletonLifetime();
        // });

        return services;
    }

    private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        return services.Scan(scanner =>
        {
            scanner.FromAssemblyOf<IApplicationAssembly>()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces().WithScopedLifetime();
        });
    }

    private static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        return services.Scan(scanner =>
        {
            scanner.FromAssemblyOf<IApplicationAssembly>()
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces().WithScopedLifetime();
        });
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IApplicationAssembly>(includeInternalTypes: true);

        return services;
    }

    private static IServiceCollection AddApiClients(this IServiceCollection services, ApplicationOptionsBuilder options)
    {
        services.AddRefitClient<ITheaterApiClient>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(options.TheaterApiUrl));

        return services;
    }

    private static IServiceCollection AddSqlConnection(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        services.AddScoped<ISqlConnection, SqlConnectionFacade>();

        return services;
    }

    private static IApplicationBuilder UseIntegrationEvents(this IApplicationBuilder appBuilder)
    {
        var services = appBuilder.ApplicationServices;

        var eventBus = services.GetRequiredService<IEventsBus>();
        var logger = services.GetRequiredService<ILogger<IApplicationAssembly>>();

        var hostLifetime = services.GetRequiredService<IHostApplicationLifetime>();
        hostLifetime.ApplicationStarted.Register(() =>
        {
            const string queueName = "tickets";
            
            eventBus.SubscribeAsync(queueName, services.GetRequiredService<ShowCreatedIntegrationEventHandler>())
                .Forget(logger);
        });

        return appBuilder;
    }
}