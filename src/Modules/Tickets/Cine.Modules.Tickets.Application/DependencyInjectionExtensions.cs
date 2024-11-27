using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Database;
using Cine.Shared.Application.Queries;
using Cine.Shared.Application.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Tickets.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Action<ApplicationOptionsBuilder> builder)
        {
            var options = new ApplicationOptionsBuilder();
            builder(options);

            services.AddMediatR(c => c.AddOpenBehavior(typeof(ValidationBehavior<,>)).RegisterServicesFromAssemblyContaining<IApplicationAssembly>());
            services.AddCommandHandlers();
            services.AddQueryHandlers();
            services.AddValidators();

            services.AddSqlConnection(options.ConnectionString);

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
    }
}
