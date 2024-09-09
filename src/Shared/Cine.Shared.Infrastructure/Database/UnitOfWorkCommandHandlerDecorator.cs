using MediatR;

namespace Cine.Shared.Infrastructure.Database
{
    public sealed class UnitOfWorkCommandHandlerDecorator<TRequest, TResult>(
        IUnitOfWork _unitOfWork,
        IRequestHandler<TRequest, TResult> _requestHandler) : IRequestHandler<TRequest, TResult>
            where TRequest : IRequest<TResult>
    {
        public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var result = await _requestHandler.Handle(request, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
    }
}
