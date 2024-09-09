using Cine.Modules.Movies.Domain;
using Cine.Modules.Movies.Infrastructure.Database.Write;
using Cine.Shared.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Movies.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<InfrastructureOptionsBuilder> builder)
        {
            var options = new InfrastructureOptionsBuilder();
            builder(options);

            services.AddUnitOfWork();
            services.AddDbContext<WriteContext>(builder => builder.UseSqlServer(options.ConnectionString));

            services.AddScoped<IPeopleRepository, PeopleRespository>();
            services.AddScoped<IMoviesRepository, MoviesRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder appBuilder)
        {
            appBuilder.ApplyMigrations();

            return appBuilder;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, WriteUnitOfWork>();
            services.Decorate(typeof(IRequestHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));

            return services;
        }

        private static IApplicationBuilder ApplyMigrations(this IApplicationBuilder appBuilder)
        {
            using var scope = appBuilder.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WriteContext>();

            context.Database.Migrate();

            return appBuilder;
        }
    }
}
