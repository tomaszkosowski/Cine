using Cine.Modules.Tickets.Application;
using Cine.Modules.Tickets.Infrastructure;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddFastEndpoints()
    .AddApplication(opts =>
    {
        opts.ConnectionString = configuration["Database:MsSql:ConnectionString"]!;
        opts.TheaterApiUrl = configuration["ApiClients:Theater"]!;
    })
    .AddInfrastructure(opts =>
    {
        opts.MsSqlConnectionString = configuration["Database:MsSql:ConnectionString"]!;
        opts.RabbitMqConnectionString = configuration["EventsBus:RabbitMq:ConnectionString"]!;
    });

var application = builder.Build();

application
    .UseFastEndpoints()
    .UseApplication()
    .UseInfrastructure()
    .UseDefaultExceptionHandler();

application.Run();

namespace Cine.Modules.Tickets.Api
{
    public partial class Program { }
}