namespace GeminiOrderService.Application.Common.Messaging;

public interface IEventProcessor<TEvent>
{
    Task ProcessEventAsync(TEvent @event, CancellationToken cancellationToken);
}