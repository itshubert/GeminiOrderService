using ErrorOr;

namespace GeminiOrderService.Domain.Common.Errors;


public static partial class Errors
{
    public static class Order
    {
        public static Error NotFound => Error.NotFound(
            code: "Order.NotFound",
            description: "The specified order was not found.");

        public static Error InvalidProductId(Guid productId) => Error.Validation(
            code: "Order.InvalidProductId",
            description: $"The product with ID '{productId}' is invalid or does not exist.");

        public static Error UnitPriceMismatch(Guid productId, decimal expectedPrice, decimal actualPrice) => Error.Validation(
            code: "Order.UnitPriceMismatch",
            description: $"The unit price for product with ID '{productId}' is invalid. Expected: {expectedPrice}, Actual: {actualPrice}.");
    }
}