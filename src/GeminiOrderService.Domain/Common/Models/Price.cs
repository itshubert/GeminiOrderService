using ErrorOr;

namespace GeminiOrderService.Domain.Common.Models;

public sealed class Price
{
    private Price(decimal amount) => Value = amount;

    public decimal Value { get; private set; }

    public static ErrorOr<Price> Create(decimal amount)
    {
        if (amount < 0)
        {
            return Error.Validation(
                code: "Price.Invalid",
                description: "The price cannot be negative.");
        }

        return new Price(amount);
    }
}