using GeminiOrderService.Domain.Models;

namespace GeminiOrderService.Domain.Orders.ValueObjects;

public sealed class OrderItemId : ValueObject
{
    public Guid Value { get; }

    private OrderItemId(Guid value)
    {
        Value = value;
    }

    public static OrderItemId CreateUnique()
    {
        return new OrderItemId(Guid.NewGuid());
    }

    public static OrderItemId Create(Guid value)
    {
        return new OrderItemId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public OrderItemId() {}
}