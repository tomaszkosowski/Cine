using Cine.Modules.Movies.Application;
using Cine.Modules.Movies.Infrastructure;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddFastEndpoints()
    .AddApplication()
    .AddInfrastructure(opts => opts.ConnectionString = configuration["Database:MsSql:ConnectionString"]!);

var app = builder.Build();

app.UseFastEndpoints();

app.Run();

public partial class Program { }