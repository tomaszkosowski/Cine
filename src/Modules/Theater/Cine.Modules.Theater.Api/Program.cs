using Cine.Modules.Theater.Application;
using Cine.Modules.Theater.Infrastructure;
using FastEndpoints;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddFastEndpoints()
    .AddApplication(opts => opts.ConnectionString = configuration["Database:MsSql:ConnectionString"]!)
    .AddInfrastructure(opts =>
    {
        opts.RabbitMqConnectionString = configuration["EventsBus:RabbitMq:ConnectionString"]!;
        opts.MsSqlConnectionString = configuration["Database:MsSql:ConnectionString"]!;
    })
    .AddOpenTelemetry()
    .ConfigureResource(res => res.AddService("Cine.Modules.Theater.Api"))
    .WithTracing(providerBuilder =>
        providerBuilder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation(opts => opts.SetDbStatementForText = true)
            .AddOtlpExporter());

var application = builder.Build();

application
    .UseFastEndpoints()
    .UseInfrastructure()
    .UseDefaultExceptionHandler();

application.Run();


namespace Cine.Modules.Theater.Api
{
    public partial class Program { }
}