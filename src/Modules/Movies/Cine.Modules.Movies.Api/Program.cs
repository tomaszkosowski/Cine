using Cine.Modules.Movies.Application;
using Cine.Modules.Movies.Infrastructure;
using FastEndpoints;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddFastEndpoints()
    .AddApplication(opts => opts.MsSqlConnectionString = configuration["Database:MsSql:ConnectionString"]!)
    .AddInfrastructure(opts =>
    {
        opts.MsSqlConnectionString = configuration["Database:MsSql:ConnectionString"]!;
        opts.RabbitMqConnectionString = configuration["EventsBus:RabbitMq:ConnectionString"]!;
    })
    .AddOpenTelemetry()
    .ConfigureResource(res => res.AddService("Cine.Modules.Movies.Api"))
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


namespace Cine.Modules.Movies.Api
{
    public partial class Program;
}