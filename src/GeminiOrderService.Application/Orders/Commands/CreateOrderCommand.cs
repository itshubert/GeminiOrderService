using ErrorOr;
using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Application.Common.Models.Orders;
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
    string ProductName,
    int Quantity,
    decimal UnitPrice);

public sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IMapper mapper
) : IRequestHandler<CreateOrderCommand, ErrorOr<OrderModel>>
{
    public async Task<ErrorOr<OrderModel>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        // Create order items first
        List<OrderItem> orderItems = new();
        List<Error> allErrors = new();

        foreach (var itemCommand in request.Items)
        {
            var subTotal = itemCommand.Quantity * itemCommand.UnitPrice;

            var orderItemResult = OrderItem.Create(
                itemCommand.ProductId,
                itemCommand.Quantity,
                itemCommand.UnitPrice,
                subTotal,
                itemCommand.ProductName);

            if (orderItemResult.IsError)
            {
                allErrors.AddRange(orderItemResult.Errors);
            }
            else
            {
                orderItems.Add(orderItemResult.Value);
            }
        }

        if (allErrors.Count > 0)
        {
            return allErrors;
        }

        // Create order with items
        var orderResult = Order.CreateWithItems(
            id: null, // Will generate unique ID
            request.CustomerId,
            DateTimeOffset.UtcNow,
            "Pending", // Default status
            request.Currency,
            orderItems);

        if (orderResult.IsError)
        {
            return orderResult.Errors;
        }

        var order = orderResult.Value;

        // Save to repository
        await orderRepository.CreateOrderAsync(order, cancellationToken);

        // Map to response model
        var orderModel = mapper.Map<OrderModel>(order);
        return orderModel;
    }
}