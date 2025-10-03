namespace GeminiOrderService.Application.Common.Messaging;

public interface IEventBridgePublisher
{
    Task PublishAsync<T>(
        string detailType,
        T eventDetail,
        CancellationToken cancellationToken = default) where T : class;
}
