using Cine.Modules.Sales.Domain;
using MassTransit;

namespace Cine.Modules.Sales.Application.Sagas;

internal sealed class PaymentState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public Guid ReservationId { get; set; }
    public Payment Payment { get; set; }
    public string CurrentState { get; set; }
}