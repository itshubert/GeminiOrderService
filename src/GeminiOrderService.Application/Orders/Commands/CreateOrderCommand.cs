using ErrorOr;
using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Application.Common.Models.Orders;
using GeminiOrderService.Application.Common.Models.Products;
using GeminiOrderService.Domain.Common.Errors;
using GeminiOrderService.Domain.Orders;
using GeminiOrderService.Domain.Orders.Entities;
using MapsterMapper;
using MediatR;

namespace GeminiOrderService.Application.Orders.Commands;

public sealed record CreateOrderCommand(
    Guid CustomerId,
    string Currency,
    IEnumerable<CreateOrderItemCommand> Items) : IRequest<ErrorOr<OrderModel>>;

public sealed record CreateOrderItemCommand(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice);

public sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    ICatalogService catalogService,
    IMapper mapper
) : IRequestHandler<CreateOrderCommand, ErrorOr<OrderModel>>
{
    public async Task<ErrorOr<OrderModel>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        // First, validate all items and collect product information
        List<(CreateOrderItemCommand itemCommand, ProductSummaryModel product)> validatedItems = new();
        List<Error> allErrors = new();
        decimal totalAmount = 0;

        foreach (var itemCommand in request.Items)
        {
            // Validate product exists
            var product = await catalogService.GetProductInfoAsync(itemCommand.ProductId, cancellationToken);
            if (product == null)
            {
                allErrors.Add(Errors.Order.InvalidProductId(itemCommand.ProductId));
                continue;
            }

            if (itemCommand.UnitPrice != product.Price)
            {
                allErrors.Add(Errors.Order.UnitPriceMismatch(itemCommand.ProductId, itemCommand.UnitPrice, product.Price));
                continue;
            }

            validatedItems.Add((itemCommand, product));
            totalAmount += itemCommand.Quantity * itemCommand.UnitPrice;
        }

        if (allErrors.Count > 0)
        {
            return allErrors;
        }

        // Create the order first
        var orderResult = Order.Create(
            id: null, // Will generate unique ID
            request.CustomerId,
            totalAmount,
            DateTimeOffset.UtcNow,
            "Pending", // Default status
            request.Currency);

        if (orderResult.IsError)
        {
            return orderResult.Errors;
        }

        var order = orderResult.Value;

        // Now create order items with the order ID
        foreach (var (itemCommand, product) in validatedItems)
        {
            var subTotal = itemCommand.Quantity * itemCommand.UnitPrice;

            var orderItemResult = OrderItem.Create(
                order.Id,
                itemCommand.ProductId,
                itemCommand.Quantity,
                itemCommand.UnitPrice,
                subTotal,
                product.Name);

            if (orderItemResult.IsError)
            {
                return orderItemResult.Errors;
            }

            order.AddItem(orderItemResult.Value);
        }

        // Save to repository
        await orderRepository.CreateOrderAsync(order, cancellationToken);

        // Map to response model
        var orderModel = mapper.Map<OrderModel>(order);
        return orderModel;
    }
}