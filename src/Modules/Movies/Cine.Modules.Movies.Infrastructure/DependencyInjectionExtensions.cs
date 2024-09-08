using Cine.Modules.Movies.Domain;
using Cine.Modules.Movies.Infrastructure.Database.Write;
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

            services.AddDbContext<WriteContext>(builder => builder.UseSqlServer(options.ConnectionString));

            services.AddScoped<IPeopleRepository, PeopleRespository>();

            return services;
        }

    }
}
