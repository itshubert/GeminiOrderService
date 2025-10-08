namespace GeminiOrderService.Infrastructure.Messaging.Events;

public sealed record InventoryReservedEvent(
    Guid OrderId,
    ShippingAddress ShippingAddress,
    IEnumerable<InventoryReservedItem> Items);

public sealed record InventoryReservedItem(
    Guid ProductId,
    int Quantity,
    string ProductName);

public sealed record ShippingAddress(
    string FirstName,
    string LastName,
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string PostCode,
    string Country);