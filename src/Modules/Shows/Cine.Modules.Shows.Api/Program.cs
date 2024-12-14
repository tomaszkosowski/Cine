using Cine.Modules.Shows.Application;
using Cine.Modules.Shows.Infrastructure;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddFastEndpoints()
    .AddApplication(opts => opts.ConnectionString = configuration["Database:MsSql:ConnectionString"]!)
    .AddInfrastructure(opts => opts.ConnectionString = configuration["Database:MsSql:ConnectionString"]!);

var application = builder.Build();

application
    .UseFastEndpoints()
    .UseApplication()
    .UseInfrastructure()
    .UseDefaultExceptionHandler();

application.Run();

namespace Cine.Modules.Shows.Api
{
    public partial class Program
    {
    }
}