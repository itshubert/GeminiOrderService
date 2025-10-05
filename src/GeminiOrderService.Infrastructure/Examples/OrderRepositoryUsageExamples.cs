using System.Linq.Expressions;
using GeminiOrderService.Domain.Orders;
using GeminiOrderService.Infrastructure.Persistence.Repositories;

namespace GeminiOrderService.Infrastructure.Examples;

/// <summary>
/// Examples of how to use the OrderRepository filtering functionality
/// </summary>
public static class OrderRepositoryUsageExamples
{
    /// <summary>
    /// Example: Get all orders for a specific customer
    /// </summary>
    public static async Task<(int TotalRecords, IEnumerable<Order> Orders)> GetOrdersByCustomer(
        OrderRepository repository,
        Guid customerId,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var filter = OrderFilters.ByCustomerId(customerId);
        return await repository.GetOrdersAsync(pageNumber, pageSize, filter);
    }

    /// <summary>
    /// Example: Get orders with specific status
    /// </summary>
    public static async Task<(int TotalRecords, IEnumerable<Order> Orders)> GetOrdersByStatus(
        OrderRepository repository,
        OrderStatus status,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var filter = OrderFilters.ByStatus(status);
        return await repository.GetOrdersAsync(pageNumber, pageSize, filter);
    }

    /// <summary>
    /// Example: Get orders within a date range
    /// </summary>
    public static async Task<(int TotalRecords, IEnumerable<Order> Orders)> GetOrdersByDateRange(
        OrderRepository repository,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var filter = OrderFilters.ByDateRange(startDate, endDate);
        return await repository.GetOrdersAsync(pageNumber, pageSize, filter);
    }

    /// <summary>
    /// Example: Get orders with amount greater than specified value
    /// </summary>
    public static async Task<(int TotalRecords, IEnumerable<Order> Orders)> GetHighValueOrders(
        OrderRepository repository,
        decimal minAmount,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var filter = OrderFilters.ByMinAmount(minAmount);
        return await repository.GetOrdersAsync(pageNumber, pageSize, filter);
    }

    /// <summary>
    /// Example: Complex filtering - Get orders for a customer within date range and amount range
    /// </summary>
    public static async Task<(int TotalRecords, IEnumerable<Order> Orders)> GetOrdersWithComplexFilter(
        OrderRepository repository,
        Guid customerId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        decimal minAmount,
        decimal maxAmount,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var customerFilter = OrderFilters.ByCustomerId(customerId);
        var dateFilter = OrderFilters.ByDateRange(startDate, endDate);
        var amountFilter = OrderFilters.ByAmountRange(minAmount, maxAmount);

        // Combine all filters with AND logic
        var combinedFilter = OrderFilters.CombineWithAnd(customerFilter, dateFilter, amountFilter);

        return await repository.GetOrdersAsync(pageNumber, pageSize, combinedFilter);
    }

    /// <summary>
    /// Example: Custom expression - Get orders for multiple customers
    /// </summary>
    public static async Task<(int TotalRecords, IEnumerable<Order> Orders)> GetOrdersForMultipleCustomers(
        OrderRepository repository,
        List<Guid> customerIds,
        int pageNumber = 1,
        int pageSize = 10)
    {
        // Custom expression using LINQ
        Expression<Func<Order, bool>> filter = order => customerIds.Contains(order.CustomerId);

        return await repository.GetOrdersAsync(pageNumber, pageSize, filter);
    }

    /// <summary>
    /// Example: Using OR logic - Get orders with specific statuses
    /// </summary>
    public static async Task<(int TotalRecords, IEnumerable<Order> Orders)> GetOrdersWithMultipleStatuses(
        OrderRepository repository,
        List<OrderStatus> statuses,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var statusFilters = statuses.Select(OrderFilters.ByStatus).ToArray();
        var combinedFilter = OrderFilters.CombineWithOr(statusFilters);

        return await repository.GetOrdersAsync(pageNumber, pageSize, combinedFilter);
    }

    /// <summary>
    /// Example: No filter - Get all orders (same as before)
    /// </summary>
    public static async Task<(int TotalRecords, IEnumerable<Order> Orders)> GetAllOrders(
        OrderRepository repository,
        int pageNumber = 1,
        int pageSize = 10)
    {
        // Passing null for filter gets all orders
        return await repository.GetOrdersAsync(pageNumber, pageSize, filter: null);
    }

    /// <summary>
    /// Example: Custom lambda expression inline
    /// </summary>
    public static async Task<(int TotalRecords, IEnumerable<Order> Orders)> GetRecentHighValueOrders(
        OrderRepository repository,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var thirtyDaysAgo = DateTimeOffset.UtcNow.AddDays(-30);

        // Custom inline expression
        Expression<Func<Order, bool>> filter = order =>
            order.OrderDate >= thirtyDaysAgo &&
            order.TotalAmount.Value >= 1000m &&
            order.Status == OrderStatus.Delivered;

        return await repository.GetOrdersAsync(pageNumber, pageSize, filter);
    }
}