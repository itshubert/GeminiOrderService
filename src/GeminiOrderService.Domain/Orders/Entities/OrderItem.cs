using ErrorOr;
using GeminiOrderService.Domain.Common.Models;
using GeminiOrderService.Domain.Orders.ValueObjects;

namespace GeminiOrderService.Domain.Orders.Entities;

public sealed class OrderItem : Entity<OrderItemId>
{
    public OrderId OrderId { get; }
    public Guid ProductId { get; }
    public string ProductNameSnapshot { get; }
    public int Quantity { get; }
    public Price UnitPrice { get; }
    public Price SubTotal { get; }

    private OrderItem(
        OrderItemId id,
        OrderId orderId,
        Guid productId,
        int quantity,
        Price unitPrice,
        Price subTotal,
        string productNameSnapshot) : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        SubTotal = subTotal;
        ProductNameSnapshot = productNameSnapshot;
    }

    public static ErrorOr<OrderItem> Create(
        OrderId orderId,
        Guid productId,
        int quantity,
        decimal unitPrice,
        decimal subTotal,
        string productNameSnapshot)
    {
        List<Error> errors = new();

        if (orderId is null)
        {
            errors.Add(Error.Validation(
                code: "OrderItem.OrderId.Null",
                description: "Order ID must be provided and cannot be null."));
        }
        else if (orderId.Value == Guid.Empty)
        {
            errors.Add(Error.Validation(
                code: "OrderItem.OrderId.Empty",
                description: "Order ID cannot be empty."));
        }

        if (productId == Guid.Empty)
        {
            errors.Add(Error.Validation(
                code: "OrderItem.ProductId.Invalid",
                description: "Product ID must be provided and cannot be empty."));
        }

        if (quantity <= 0)
        {
            errors.Add(Error.Validation(
                code: "OrderItem.Quantity.Invalid",
                description: "Quantity must be greater than zero."));
        }

        if (unitPrice <= 0)
        {
            errors.Add(Error.Validation(
                code: "OrderItem.UnitPrice.Invalid",
                description: "Unit price must be greater than zero."));
        }

        if (string.IsNullOrWhiteSpace(productNameSnapshot))
        {
            errors.Add(Error.Validation(
                code: "OrderItem.ProductNameSnapshot.Invalid",
                description: "Product name snapshot must be provided and cannot be empty."));
        }
        else if (productNameSnapshot.Length > 200)
        {
            errors.Add(Error.Validation(
                code: "OrderItem.ProductNameSnapshot.TooLong",
                description: "Product name snapshot cannot exceed 200 characters."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        var orderItem = new OrderItem(
            id: OrderItemId.CreateUnique(),
            orderId: orderId!, // Safe because we validated above
            productId: productId,
            quantity: quantity,
            unitPrice: Price.Create(unitPrice).Value,
            subTotal: Price.Create(subTotal).Value,
            productNameSnapshot: productNameSnapshot);

        return orderItem;
    }

#pragma warning disable CS8618
    private OrderItem() : base() { }
#pragma warning restore CS8618

}