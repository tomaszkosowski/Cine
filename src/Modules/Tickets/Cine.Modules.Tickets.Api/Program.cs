using Cine.Modules.Tickets.Application;
using Cine.Modules.Tickets.Infrastructure;
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
    .UseInfrastructure()
    .UseDefaultExceptionHandler();

application.Run();

namespace Cine.Modules.Tickets.Api
{
    public partial class Program { }
}