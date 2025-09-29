using ErrorOr;
using GeminiOrderService.Domain.Common.Models;
using GeminiOrderService.Domain.Orders.Entities;
using GeminiOrderService.Domain.Orders.ValueObjects;

namespace GeminiOrderService.Domain.Orders;

public sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderItem> _orderItems = new();

    public Guid CustomerId { get; private set; }
    public Price TotalAmount { get; private set; }
    public DateTimeOffset OrderDate { get; private set; }
    public string Status { get; private set; }
    public string Currency { get; private set; }

    public IReadOnlyList<OrderItem> Items => _orderItems.AsReadOnly();

    private Order(
        OrderId id,
        Guid customerId,
        Price totalAmount,
        DateTimeOffset orderDate,
        string status,
        string currency) : base(id)
    {
        CustomerId = customerId;
        TotalAmount = totalAmount;
        OrderDate = orderDate;
        Status = status;
        Currency = currency;
    }

    // Parameterless constructor for EF Core
#pragma warning disable CS8618
    private Order() : base() { }
#pragma warning restore CS8618

    public static ErrorOr<Order> Create(
        OrderId? id,
        Guid customerId,
        decimal totalAmount,
        DateTimeOffset orderDate,
        string status,
        string currency)
    {
        List<Error> errors = new();

        var priceOrError = Price.Create(totalAmount);
        if (priceOrError.IsError)
        {
            errors.AddRange(priceOrError.Errors);
        }

        if (string.IsNullOrWhiteSpace(status))
        {
            errors.Add(Error.Validation(
                code: "Order.Status.Invalid",
                description: "The order status cannot be empty."));
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            errors.Add(Error.Validation(
                code: "Order.Currency.Invalid",
                description: "The currency cannot be empty."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Order(
            id ?? OrderId.CreateUnique(),
            customerId,
            priceOrError.Value,
            orderDate,
            status,
            currency);
    }

    public void AddItem(OrderItem orderItem)
    {
        _orderItems.Add(orderItem);
    }

    public void RemoveItem(OrderItem orderItem)
    {
        _orderItems.Remove(orderItem);
    }

    public static ErrorOr<Order> CreateWithItems(
        OrderId? id,
        Guid customerId,
        DateTimeOffset orderDate,
        string status,
        string currency,
        IEnumerable<OrderItem> orderItems)
    {
        List<Error> errors = new();

        if (string.IsNullOrWhiteSpace(status))
        {
            errors.Add(Error.Validation(
                code: "Order.Status.Invalid",
                description: "The order status cannot be empty."));
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            errors.Add(Error.Validation(
                code: "Order.Currency.Invalid",
                description: "The currency cannot be empty."));
        }

        var orderItemsList = orderItems.ToList();
        if (!orderItemsList.Any())
        {
            errors.Add(Error.Validation(
                code: "Order.Items.Empty",
                description: "Order must have at least one item."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        // Calculate total amount from items
        var totalAmount = orderItemsList.Sum(item => item.SubTotal.Value);
        var priceResult = Price.Create(totalAmount);
        if (priceResult.IsError)
        {
            return priceResult.Errors;
        }

        var order = new Order(
            id ?? OrderId.CreateUnique(),
            customerId,
            priceResult.Value,
            orderDate,
            status,
            currency);

        // Add all items
        foreach (var item in orderItemsList)
        {
            order._orderItems.Add(item);
        }

        return order;
    }
}

