using Cine.Modules.Shows.Domain;
using Cine.Modules.Shows.Domain.Events;
using Cine.Modules.Shows.Infrastructure.Database.Write;
using Cine.Modules.Shows.Infrastructure.Outbox;
using Cine.Shared.Application.Outbox;
using Cine.Shared.Infrastructure.Database;
using Cine.Shared.Infrastructure.Events;
using Cine.Shared.Infrastructure.Jobs;
using Hangfire;
using Hangfire.Dashboard;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Shows.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        Action<InfrastructureOptionsBuilder> builder)
    {
        var options = new InfrastructureOptionsBuilder();
        builder(options);

        services.AddUnitOfWork();
        services.AddDbContext<WriteContext>(cfg => cfg.UseSqlServer(options.MsSqlConnectionString));

        services.AddOutbox();
        services.AddHangfire();
        services.AddEventsBus(options.RabbitMqConnectionString);
        services.AddRepositories();
        services.AddRecurringJobs();
        services.AddEventsDispatching();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder appBuilder)
    {
        appBuilder.ApplyMigrations();
        appBuilder.UseHangfireDashboard();
        appBuilder.TriggerRecurringJobs();

        return appBuilder;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, WriteUnitOfWork>();
        services.Decorate(typeof(IRequestHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));

        return services;
    }

    private static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        services.AddScoped<IOutbox, OutboxAccessor>();
        services.AddSingleton<IDomainEventsMapper>(_ => new DomainEventsMapper(new Dictionary<string, Type>
        {
            { nameof(ShowCreatedDomainEvent), typeof(ShowCreatedDomainEvent) }
        }));

        return services;
    }

    private static IServiceCollection AddHangfire(this IServiceCollection services)
    {
        services.AddHangfire(cfg => cfg
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseInMemoryStorage());

        services.AddHangfireServer();

        return services;
    }

    private static IServiceCollection AddEventsBus(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<RabbitMqEventsBusBackgroundService>(_ => new RabbitMqEventsBusBackgroundService(connectionString));

        services.AddSingleton<IEventsBus>(serviceProvider =>
            serviceProvider.GetRequiredService<RabbitMqEventsBusBackgroundService>());

        services.AddHostedService(serviceProvider =>
            serviceProvider.GetRequiredService<RabbitMqEventsBusBackgroundService>());
        
        return services;
    }

    private static IServiceCollection AddRecurringJobs(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddEventsDispatching(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventsCollector>(scope =>
            new DomainEventsCollector<WriteContext>(scope.GetRequiredService<WriteContext>()));
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IHallsRepository, HallsRepository>();
        services.AddScoped<IMoviesRepository, MoviesRepository>();

        return services;
    }

    private static IApplicationBuilder ApplyMigrations(this IApplicationBuilder appBuilder)
    {
        using var scope = appBuilder.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WriteContext>();
            
        context.Database.Migrate();

        return appBuilder;
    }

    private static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder appBuilder)
    {
        appBuilder.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = [new AllowAll()]
        });

        return appBuilder;
    }

    private static IApplicationBuilder TriggerRecurringJobs(this IApplicationBuilder appBuilder)
    {
        RecurringJob.AddOrUpdate<ProcessOutboxJob>(ProcessOutboxJob.JobName, job => job.ExecuteAsync(),
            "* * * * * *");

        RecurringJob.TriggerJob(ProcessOutboxJob.JobName);

        return appBuilder;
    }

    private class AllowAll : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}