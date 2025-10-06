using ErrorOr;
using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Application.Common.Mappings;
using GeminiOrderService.Application.Common.Models.Orders;
using GeminiOrderService.Domain.Common.Errors;
using MediatR;

namespace GeminiOrderService.Application.Orders.Commands;

public sealed record UpdateOrderStatusCommand(Guid OrderId, OrderStatus Status) : IRequest<ErrorOr<Success>>;

public sealed class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, ErrorOr<Success>>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderForUpdateAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        order.UpdateStatus(request.Status.ToDomainOrderStatus());

        await _orderRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
