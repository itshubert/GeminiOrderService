namespace GeminiOrderService.Infrastructure.Messaging.Events;

public sealed record LabelGeneratedEvent(
    Guid OrderId,
    string TrackingNumber
);