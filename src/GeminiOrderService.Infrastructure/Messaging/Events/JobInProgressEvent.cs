namespace GeminiOrderService.Infrastructure.Messaging.Events;

public sealed record JobInProgressEvent(
    Guid JobId,
    Guid OrderId);