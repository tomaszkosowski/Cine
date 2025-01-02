namespace Cine.Modules.Tickets.Infrastructure;

public sealed class InfrastructureOptionsBuilder
{
    public string MsSqlConnectionString { get; set; }
    
    public string RabbitMqConnectionString { get; set; }
}