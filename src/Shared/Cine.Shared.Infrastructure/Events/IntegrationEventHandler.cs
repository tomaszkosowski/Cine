using Cine.Shared.Application.Database;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Cine.Shared.Infrastructure.Events;

public abstract class IntegrationEventHandler<TIntegrationEvent>(IServiceScopeFactory serviceScopeFactory)
    : IIntegrationEventHandler<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
{
    public async Task HandleAsync(TIntegrationEvent @event)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var sqlConnection = scope.ServiceProvider.GetRequiredService<ISqlConnection>();

        const string sql = $"""
                            INSERT INTO 
                                 [dbo].[Inbox] (Id, CreatedAt, Type, Content)
                            VALUES 
                                 (@Id, @CreatedAt, @Type, @Content)
                            """;

        var type = @event.GetType().Name;
        var content = JsonConvert.SerializeObject(@event, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

        await sqlConnection.ExecuteScalarAsync(sql, new
        {
            @event.Id,
            @event.CreatedAt,
            Type = type,
            Content = content
        });
    }
}