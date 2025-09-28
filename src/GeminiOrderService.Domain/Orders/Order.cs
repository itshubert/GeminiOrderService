using ErrorOr;
using GeminiOrderService.Domain.Models;

namespace GeminiOrderService.Domain.Orders;

public sealed class Order : AggregateRoot<Guid>
{
    public Guid CustomerId { get; private set; }
    public Price TotalAmount { get; private set; }
    public DateTimeOffset OrderDate { get; private set; }
    public string Status { get; private set; }
    public string Currency { get; private set; }

    private Order(
        Guid id,
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
        Guid? id,
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
            id ?? Guid.NewGuid(),
            customerId,
            priceOrError.Value,
            orderDate,
            status,
            currency);
    }
}

