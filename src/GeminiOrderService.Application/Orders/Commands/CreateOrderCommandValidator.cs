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

        RuleFor(x => x.ShippingAddress)
            .NotNull().WithMessage("Shipping address is required.")
            .ChildRules(address =>
            {
                address.RuleFor(a => a.FirstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

                address.RuleFor(a => a.LastName)
                    .NotEmpty().WithMessage("Last name is required.")
                    .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

                address.RuleFor(a => a.AddressLine1)
                    .NotEmpty().WithMessage("Address line 1 is required.")
                    .MaximumLength(200).WithMessage("Address line 1 cannot exceed 200 characters.");

                address.RuleFor(a => a.City)
                    .NotEmpty().WithMessage("City is required.")
                    .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

                address.RuleFor(a => a.State)
                    .NotEmpty().WithMessage("State is required.")
                    .MaximumLength(100).WithMessage("State cannot exceed 100 characters.");

                address.RuleFor(a => a.PostCode)
                    .NotEmpty().WithMessage("Post code is required.")
                    .MaximumLength(20).WithMessage("Post code cannot exceed 20 characters.");

                address.RuleFor(a => a.Country)
                    .NotEmpty().WithMessage("Country is required.")
                    .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.");
            });
    }
}