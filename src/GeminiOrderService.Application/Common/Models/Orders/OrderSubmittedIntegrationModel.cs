using GeminiOrderService.Application.Common.Models.Orders;

namespace GeminiOrderService.Application.Common.Models.Ordersl;

public sealed record OrderSubmittedIntegrationModel(
    Guid Id,
    Guid CustomerId,
    DateTimeOffset OrderDate,
    string Status,
    decimal TotalAmount,
    string Currency,
    IEnumerable<OrderItemModel> Items,
    ShippingAddress ShippingAddress);

public sealed record ShippingAddress(
    string FirstName,
    string LastName,
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string PostCode,
    string Country);