using Cine.Shared.Application.Database;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Outbox;
using Cine.Shared.Domain;
using Cine.Shared.Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cine.Shared.Infrastructure.Jobs;

public sealed class ProcessOutboxJob(
    ISqlConnection sqlConnection,
    IDomainEventsMapper domainEventsMapper,
    IPublisher publisher,
    ILogger<ProcessOutboxJob> logger)
{
    public const string JobName = nameof(ProcessOutboxJob);

    public async Task ExecuteAsync()
    {
        const string querySql = $"""
                                 SELECT 
                                    O.[Id] AS [{nameof(OutboxMessage.Id)}],
                                    O.[Type] AS [{nameof(OutboxMessage.Type)}],
                                    O.[Content] AS {nameof(OutboxMessage.Content)}
                                 FROM [dbo].[OutboxMessages] O
                                 WHERE O.[ProcessedAt] IS NULL
                                 """;

        var outboxMessages = await sqlConnection.QueryAsync<OutboxMessage>(querySql);
        foreach (var outboxMessage in outboxMessages)
        {
            try
            {
                var type = domainEventsMapper.GetDomainEventType(outboxMessage.Type);
                var notification = JsonConvert.DeserializeObject(outboxMessage.Content, type)!;

                await publisher.Publish((INotification)notification);

                const string insertSql = $"""
                                          UPDATE [dbo].[OutboxMessages] 
                                          SET [ProcessedAt] = @ProcessedAt
                                          WHERE [Id] = @Id
                                          """;

                await sqlConnection.ExecuteScalarAsync(insertSql, new
                {
                    Id = outboxMessage.Id,
                    ProcessedAt = Utc.Now
                });
            }
            catch (Exception ex)
            {
                logger.LogInfrastructureError(ex);
            }
        }
    }
}