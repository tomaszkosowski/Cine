using MassTransit;

namespace Cine.Shared.Infrastructure.Database;

public sealed class UnitOfWorkConsumerFilter<TMessage>(IUnitOfWork unitOfWork)
    : IFilter<ConsumeContext<TMessage>> where TMessage : class
{
    public async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
    {
        await next.Send(context);

        var result = await unitOfWork.CommitAsync(context.CancellationToken);
    }

    public void Probe(ProbeContext context)
    {
    }
}