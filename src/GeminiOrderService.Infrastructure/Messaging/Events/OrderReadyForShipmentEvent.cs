namespace GeminiOrderService.Infrastructure.Messaging.Events;

public sealed record OrderReadyForShipmentEvent(
    Guid OrderId,
    string TrackingNumber
);