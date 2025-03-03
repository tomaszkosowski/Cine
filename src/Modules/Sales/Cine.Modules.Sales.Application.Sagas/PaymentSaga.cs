using Cine.Modules.Sales.Application.Sagas.Consumers;
using MassTransit;

namespace Cine.Modules.Sales.Application.Sagas;

internal sealed class PaymentSaga : MassTransitStateMachine<PaymentState>
{
    public State PaymentPending { get; private set; }

    public Event<ReservationConfirmed> ReservationConfirmed { get; private set; }
    public Event<PaymentCreated> PaymentCreated { get; private set; }
    public Event<PaymentSucceeded> PaymentSucceeded { get; private set; }
    public Event<PaymentFailed> PaymentFailed { get; private set; }

    public PaymentSaga()
    {
        InstanceState(paymentState => paymentState.CurrentState);

        Event(() => ReservationConfirmed, correlation => correlation.CorrelateById(context => context.Message.ReservationId));
        Event(() => PaymentCreated, correlation => correlation.CorrelateById(context => context.Message.ReservationId));
        Event(() => PaymentSucceeded, correlation => correlation.CorrelateById(context => context.Message.ReservationId));
        Event(() => PaymentFailed, correlation => correlation.CorrelateById(context => context.Message.ReservationId));

        Initially(
            When(ReservationConfirmed)
                .Then(context => context.Saga.ReservationId = context.Message.ReservationId)
                .Publish(context => new CreatePayment(context.Saga.ReservationId))
                .TransitionTo(PaymentPending));

        During(PaymentPending,
            When(PaymentCreated)
                .Publish(context => new ProcessPayment(context.Saga.ReservationId)),
            When(PaymentSucceeded)
                .Publish(context => new ConfirmPayment(context.Saga.ReservationId))
                .Finalize(),
            When(PaymentFailed)
                .Publish(context => new CancelPayment(context.Saga.ReservationId))
                .Finalize());

        SetCompletedWhenFinalized();
    }
}