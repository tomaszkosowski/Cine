using Cine.Modules.Sales.Application.Sagas;
using Cine.Shared.Application.Commands;
using MassTransit;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Sales.Application.Payments.CreatePayment;

public class CreatePaymentCommandHandler(IPublishEndpoint publishEndpoint)
    : ICommandHandler<CreatePaymentCommand, OneOf<Success, Error<ApplicationException>>>
{
    public async Task<OneOf<Success, Error<ApplicationException>>> Handle(CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new ReservationConfirmed(request.ReservationId), cancellationToken);

        return new Success();
    }
}