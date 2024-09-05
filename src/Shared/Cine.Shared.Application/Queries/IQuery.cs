using MediatR;

namespace Cine.Shared.Application.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
