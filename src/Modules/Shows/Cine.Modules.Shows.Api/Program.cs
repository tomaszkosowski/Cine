using Cine.Modules.Shows.Application;
using Cine.Modules.Shows.Infrastructure;
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
        opts.MsSqlConnectionString = configuration["Database:MsSql:ConnectionString"]!;
        opts.RabbitMqConnectionString = configuration["EventsBus:RabbitMq:ConnectionString"]!;
    })
    .AddOpenTelemetry()
    .ConfigureResource(res => res.AddService("Cine.Modules.Shows.Api"))
    .WithTracing(providerBuilder =>
        providerBuilder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddOtlpExporter());

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