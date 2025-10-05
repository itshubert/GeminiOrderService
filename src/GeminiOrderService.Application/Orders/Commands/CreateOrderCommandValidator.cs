using FluentValidation;

namespace GeminiOrderService.Application.Orders.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Order must contain at least one item.")
            .Must(items => items.All(item => item.Quantity > 0)).WithMessage("All order items must have a quantity greater than zero.")
            .Must(items => items.All(item => item.UnitPrice >= 0)).WithMessage("All order items must have a non-negative price.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.");
    }
}