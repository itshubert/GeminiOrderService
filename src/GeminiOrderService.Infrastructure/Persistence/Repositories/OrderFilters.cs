using System.Linq.Expressions;
using GeminiOrderService.Domain.Orders;

namespace GeminiOrderService.Infrastructure.Persistence.Repositories;

/// <summary>
/// Common filter expressions for Order queries
/// </summary>
public static class OrderFilters
{
    /// <summary>
    /// Filter orders by customer ID
    /// </summary>
    /// <param name="customerId">The customer ID to filter by</param>
    /// <returns>Expression to filter orders by customer ID</returns>
    public static Expression<Func<Order, bool>> ByCustomerId(Guid customerId)
        => order => order.CustomerId == customerId;

    /// <summary>
    /// Filter orders by status
    /// </summary>
    /// <param name="status">The status to filter by</param>
    /// <returns>Expression to filter orders by status</returns>
    public static Expression<Func<Order, bool>> ByStatus(string status)
        => order => order.Status == status;

    /// <summary>
    /// Filter orders by currency
    /// </summary>
    /// <param name="currency">The currency to filter by</param>
    /// <returns>Expression to filter orders by currency</returns>
    public static Expression<Func<Order, bool>> ByCurrency(string currency)
        => order => order.Currency == currency;

    /// <summary>
    /// Filter orders by date range
    /// </summary>
    /// <param name="startDate">The start date (inclusive)</param>
    /// <param name="endDate">The end date (inclusive)</param>
    /// <returns>Expression to filter orders by date range</returns>
    public static Expression<Func<Order, bool>> ByDateRange(DateTimeOffset startDate, DateTimeOffset endDate)
        => order => order.OrderDate >= startDate && order.OrderDate <= endDate;

    /// <summary>
    /// Filter orders with total amount greater than or equal to specified amount
    /// </summary>
    /// <param name="minAmount">The minimum total amount</param>
    /// <returns>Expression to filter orders by minimum amount</returns>
    public static Expression<Func<Order, bool>> ByMinAmount(decimal minAmount)
        => order => order.TotalAmount.Value >= minAmount;

    /// <summary>
    /// Filter orders with total amount less than or equal to specified amount
    /// </summary>
    /// <param name="maxAmount">The maximum total amount</param>
    /// <returns>Expression to filter orders by maximum amount</returns>
    public static Expression<Func<Order, bool>> ByMaxAmount(decimal maxAmount)
        => order => order.TotalAmount.Value <= maxAmount;

    /// <summary>
    /// Filter orders with total amount within specified range
    /// </summary>
    /// <param name="minAmount">The minimum total amount</param>
    /// <param name="maxAmount">The maximum total amount</param>
    /// <returns>Expression to filter orders by amount range</returns>
    public static Expression<Func<Order, bool>> ByAmountRange(decimal minAmount, decimal maxAmount)
        => order => order.TotalAmount.Value >= minAmount && order.TotalAmount.Value <= maxAmount;

    /// <summary>
    /// Combines multiple filter expressions using AND logic
    /// </summary>
    /// <param name="filters">The filter expressions to combine</param>
    /// <returns>Combined expression using AND logic</returns>
    public static Expression<Func<Order, bool>>? CombineWithAnd(params Expression<Func<Order, bool>>?[] filters)
    {
        var validFilters = filters.Where(f => f != null).Cast<Expression<Func<Order, bool>>>().ToList();

        if (validFilters.Count == 0)
            return null;

        if (validFilters.Count == 1)
            return validFilters[0];

        var result = validFilters[0];
        for (int i = 1; i < validFilters.Count; i++)
        {
            result = CombineTwoExpressions(result, validFilters[i], Expression.AndAlso);
        }

        return result;
    }

    /// <summary>
    /// Combines multiple filter expressions using OR logic
    /// </summary>
    /// <param name="filters">The filter expressions to combine</param>
    /// <returns>Combined expression using OR logic</returns>
    public static Expression<Func<Order, bool>>? CombineWithOr(params Expression<Func<Order, bool>>?[] filters)
    {
        var validFilters = filters.Where(f => f != null).Cast<Expression<Func<Order, bool>>>().ToList();

        if (validFilters.Count == 0)
            return null;

        if (validFilters.Count == 1)
            return validFilters[0];

        var result = validFilters[0];
        for (int i = 1; i < validFilters.Count; i++)
        {
            result = CombineTwoExpressions(result, validFilters[i], Expression.OrElse);
        }

        return result;
    }

    private static Expression<Func<Order, bool>> CombineTwoExpressions(
        Expression<Func<Order, bool>> left,
        Expression<Func<Order, bool>> right,
        Func<Expression, Expression, BinaryExpression> combiner)
    {
        var parameter = Expression.Parameter(typeof(Order), "order");

        var leftBody = ReplaceParameter(left.Body, left.Parameters[0], parameter);
        var rightBody = ReplaceParameter(right.Body, right.Parameters[0], parameter);

        var combined = combiner(leftBody, rightBody);

        return Expression.Lambda<Func<Order, bool>>(combined, parameter);
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return new ParameterReplacer(oldParameter, newParameter).Visit(expression);
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}