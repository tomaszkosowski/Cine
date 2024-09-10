using Cine.Modules.Movies.Application;
using Cine.Modules.Movies.Infrastructure;
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


public partial class Program { }