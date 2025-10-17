using System.Linq.Expressions;
using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Domain.Orders;
using GeminiOrderService.Domain.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GeminiOrderService.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository : BaseRepository, IOrderRepository
{
    public OrderRepository(GeminiOrderDbContext context) : base(context)
    {
    }

    public async Task<(int TotalRecords, IEnumerable<Order> Orders)> GetOrdersAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<Order, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Order> query = _context.Orders;

        // Apply filter if provided
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        int totalRecords = await query.CountAsync(cancellationToken);

        List<Order> orders = await query
            .Include(o => o.Items)
            .OrderByDescending(o => o.OrderDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (totalRecords, orders);
    }

    public async Task<Order?> GetOrderForUpdateAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var orderIdValue = OrderId.Create(orderId);
        var order = await _context.Orders
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == orderIdValue, cancellationToken);

        return order;
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Items)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }
}