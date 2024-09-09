using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Database;
using Cine.Shared.Application.Queries;
using Cine.Shared.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Movies.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Action<ApplicationOptionsBuilder> builder)
        {
            var options = new ApplicationOptionsBuilder();
            builder(options);

            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(IApplicationAssembly).Assembly));
            services.AddCommandHandlers();
            services.AddQueryHandlers();

            services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(options.ConnectionString));
            services.AddScoped<ISqlConnection, SqlConnectionFacade>();

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
    }
}
