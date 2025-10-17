namespace GeminiOrderService.Infrastructure.Messaging.Events;

public sealed record OrderInProgressEvent(
    Guid JobId,
    Guid OrderId);