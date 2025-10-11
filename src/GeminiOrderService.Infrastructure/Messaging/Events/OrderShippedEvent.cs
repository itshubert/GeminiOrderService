namespace GeminiOrderService.Infrastructure.Messaging.Events;

public sealed record OrderShippedEvent(
    Guid OrderId,
    string TrackingNumber);