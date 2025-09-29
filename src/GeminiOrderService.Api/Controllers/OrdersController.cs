using GeminiOrderService.Application.Orders.Queries;
using GeminiOrderService.Contracts;
using GeminiOrderService.Contracts.Orders;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiOrderService.Api.Controllers;

[Route("[controller]")]
public sealed class OrdersController : ApiController
{

    public OrdersController(ISender mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetOrdersQuery(
            pageNumber,
            pageSize,
            cancellationToken);

        var response = await Mediator.Send(query, cancellationToken);
        var items = Mapper.Map<IEnumerable<OrderResponse>>(response.Items);

        return Ok(new PagedResultResponse<OrderResponse>(
            items,
            response.TotalCount,
            response.PageNumber,
            response.PageSize));
    }

}