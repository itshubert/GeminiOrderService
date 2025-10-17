using System.Linq.Expressions;
using GeminiOrderService.Domain.Orders;

namespace GeminiOrderService.Application.Common.Interfaces;

public interface IOrderRepository : IRepository
{
    Task<(int TotalRecords, IEnumerable<Order> Orders)> GetOrdersAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<Order, bool>>? filter = null,
        CancellationToken cancellationToken = default);

    Task<Order?> GetOrderForUpdateAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken = default);
}