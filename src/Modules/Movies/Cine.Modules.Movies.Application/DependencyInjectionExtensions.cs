using Cine.Shared.Application.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Movies.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(IApplicationAssembly).Assembly));
            services.AddCommandHandlers();

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
    }
}
