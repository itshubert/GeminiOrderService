using System.Linq.Expressions;
using GeminiOrderService.Domain.Orders;

namespace GeminiOrderService.Application.Common.Interfaces;

public interface IOrderRepository
{
    Task<(int TotalRecords, IEnumerable<Order> Orders)> GetOrdersAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<Order, bool>>? filter = null,
        CancellationToken cancellationToken = default);
}