using Cine.Modules.Theater.Application;
using Cine.Modules.Theater.Infrastructure;
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


namespace Cine.Modules.Theater.Api
{
    public partial class Program { }
}