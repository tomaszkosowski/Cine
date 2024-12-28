using MediatR;

namespace Cine.Shared.Infrastructure.Database;

public sealed class UnitOfWorkCommandHandlerDecorator<TRequest, TResult>(
    IUnitOfWork unitOfWork,
    IRequestHandler<TRequest, TResult> requestHandler) : IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var result = await requestHandler.Handle(request, cancellationToken);

        var changes = await unitOfWork.CommitAsync(cancellationToken);

        return result;
    }
}