using System.Linq.Expressions;
using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Domain.Orders;
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
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (totalRecords, orders);
    }
}