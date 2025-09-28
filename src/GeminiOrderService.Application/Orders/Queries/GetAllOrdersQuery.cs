using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Application.Common.Models.Orders;
using MapsterMapper;
using MediatR;

namespace GeminiOrderService.Application.Orders.Queries;

public sealed record GetAllOrdersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    CancellationToken CancellationToken = default
) : IRequest<IEnumerable<OrderModel>>;

public sealed class GetAllOrdersQueryHandler(
    IOrderRepository orderRepository,
    IMapper mapper
) : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderModel>>
{
    public async Task<IEnumerable<OrderModel>> Handle(
        GetAllOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var (totalRecords, orders) = await orderRepository.GetOrdersAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken: request.CancellationToken);


        var ordersResponse = mapper.Map<IEnumerable<OrderModel>>(orders);

        return ordersResponse;
    }
}

