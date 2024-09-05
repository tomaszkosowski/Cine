using OneOf;
using MediatR;
using OneOf.Types;

namespace Cine.Shared.Application.Queries
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}
