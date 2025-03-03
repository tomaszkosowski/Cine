using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Sales.Application.Payments.CreatePayment;

public record CreatePaymentCommand(Guid ReservationId) : Command<OneOf<Success, Error<ApplicationException>>>;