using ApplicationOrderStatus = GeminiOrderService.Application.Common.Models.Orders.OrderStatus;
using DomainOrderStatus = GeminiOrderService.Domain.Orders.OrderStatus;

namespace GeminiOrderService.Application.Common.Mappings;

public static class OrderStatusMappingExtensions
{
    /// <summary>
    /// Converts Application layer OrderStatus to Domain layer OrderStatus
    /// </summary>
    /// <param name="applicationStatus">The application layer order status</param>
    /// <returns>The corresponding domain layer order status</returns>
    public static DomainOrderStatus ToDomainOrderStatus(this ApplicationOrderStatus applicationStatus)
    {
        return applicationStatus switch
        {
            ApplicationOrderStatus.Pending => DomainOrderStatus.Pending,
            ApplicationOrderStatus.Confirmed => DomainOrderStatus.Confirmed,
            ApplicationOrderStatus.Shipped => DomainOrderStatus.Shipped,
            ApplicationOrderStatus.Delivered => DomainOrderStatus.Delivered,
            ApplicationOrderStatus.Cancelled => DomainOrderStatus.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(applicationStatus), applicationStatus,
                $"Unknown order status: {applicationStatus}")
        };
    }

    /// <summary>
    /// Converts Domain layer OrderStatus to Application layer OrderStatus
    /// </summary>
    /// <param name="domainStatus">The domain layer order status</param>
    /// <returns>The corresponding application layer order status</returns>
    public static ApplicationOrderStatus ToApplicationOrderStatus(this DomainOrderStatus domainStatus)
    {
        return domainStatus switch
        {
            DomainOrderStatus.Pending => ApplicationOrderStatus.Pending,
            DomainOrderStatus.Confirmed => ApplicationOrderStatus.Confirmed,
            DomainOrderStatus.Shipped => ApplicationOrderStatus.Shipped,
            DomainOrderStatus.Delivered => ApplicationOrderStatus.Delivered,
            DomainOrderStatus.Cancelled => ApplicationOrderStatus.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(domainStatus), domainStatus,
                $"Unknown order status: {domainStatus}")
        };
    }
}