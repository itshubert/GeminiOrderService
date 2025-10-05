namespace GeminiOrderService.Infrastructure.Messaging.Events;

public sealed record InventoryReservedEvent(
    Guid OrderId,
    IEnumerable<InventoryReservedItem> Items);

public sealed record InventoryReservedItem(
    Guid ProductId,
    int Quantity,
    string ProductName);