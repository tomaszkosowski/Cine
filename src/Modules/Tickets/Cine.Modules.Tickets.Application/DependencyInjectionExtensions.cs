using Cine.Modules.Tickets.Application.Shows;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Database;
using Cine.Shared.Application.Queries;
using Cine.Shared.Application.Validation;
using Cine.Shared.Infrastructure.Events;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Tickets.Application
{
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
            services.AddSingleton<ShowAddedIntegrationEventHandler>();

            // return services.Scan(scanner =>
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

            eventBus.Subscribe(services.GetRequiredService<ShowAddedIntegrationEventHandler>());

            return appBuilder;
        }
    }
}