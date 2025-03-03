using Cine.Modules.Sales.Application;
using Cine.Modules.Sales.Infrastructure;
using FastEndpoints;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddFastEndpoints()
    .AddApplication(opts =>
    {
        opts.ConnectionString = configuration["Database:MsSql:ConnectionString"]!;
        opts.TicketsApiUrl = configuration["ApiClients:Tickets"]!;
    })
    .AddInfrastructure(opts =>
    {
        opts.RabbitMqConnectionString = configuration["EventsBus:RabbitMq:ConnectionString"]!;
        opts.MsSqlConnectionString = configuration["Database:MsSql:ConnectionString"]!;
    })
    .AddOpenTelemetry()
    .ConfigureResource(res => res.AddService("Cine.Modules.Sales.Api"))
    .WithTracing(providerBuilder =>
        providerBuilder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation(opts => opts.SetDbStatementForText = true)
            .AddOtlpExporter());

var application = builder.Build();

application
    .UseFastEndpoints()
    .UseApplication()
    .UseInfrastructure()
    .UseDefaultExceptionHandler();

application.Run();

namespace Cine.Modules.Sales.Api
{
    public partial class Program;
}