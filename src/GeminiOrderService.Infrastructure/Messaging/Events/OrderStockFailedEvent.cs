namespace GeminiOrderService.Infrastructure.Messaging.Events;

public sealed record OrderStockFailedEvent(
    Guid OrderId,
    IEnumerable<OrderStockFailedItem> FailedItems);

public sealed record OrderStockFailedItem(
    Guid ProductId,
    int Quantity,
    string Reason);