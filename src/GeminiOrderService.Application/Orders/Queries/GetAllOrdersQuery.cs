using ErrorOr;
using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Application.Common.Models.Orders;
using Mapster;
using MediatR;

namespace GeminiOrderService.Application.Orders.Queries;

public sealed record GetAllOrdersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    CancellationToken CancellationToken = default
) : IRequest<ErrorOr<List<OrderModel>>>;

public sealed class GetAllOrdersQueryHandler(
    IOrderRepository orderRepository
) : IRequestHandler<GetAllOrdersQuery, ErrorOr<List<OrderModel>>>
{
    public async Task<ErrorOr<List<OrderModel>>> Handle(
        GetAllOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var (totalRecords, orders) = await orderRepository.GetOrdersAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken: request.CancellationToken);

        var orderModels = orders.Select(order => order.Adapt<OrderModel>()).ToList();

        return orderModels;
    }
}

