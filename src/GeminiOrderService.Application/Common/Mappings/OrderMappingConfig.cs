using System.Linq;
using GeminiOrderService.Application.Common.Models.Orders;
using GeminiOrderService.Application.Common.Models.Ordersl;
using GeminiOrderService.Domain.Orders;
using GeminiOrderService.Domain.Orders.Entities;
using Mapster;

namespace GeminiOrderService.Application.Common.Mappings;

public class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderItem, OrderItemModel>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ProductName, src => src.ProductNameSnapshot)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice.Value)
            .Map(dest => dest.SubTotal, src => src.SubTotal.Value)
            .Map(dest => dest, src => src);

        config.NewConfig<Order, OrderModel>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.TotalAmount, src => src.TotalAmount.Value)
            .Map(dest => dest.Items, src => src.Items.Select(item => item.Adapt<OrderItemModel>()))
            .Map(dest => dest, src => src);
    }
}

public static class OrderExtensions
{
    public static OrderSubmittedIntegrationModel ToIntegrationModel(this Order order) =>
        new OrderSubmittedIntegrationModel(
            order.Id.Value,
            order.CustomerId,
            order.OrderDate,
            order.Status.ToString(),
            order.TotalAmount.Value,
            order.Currency,
            order.Items.Select(item => item.Adapt<OrderItemModel>()),
            new ShippingAddress(
                order.ShippingAddress.FirstName,
                order.ShippingAddress.LastName,
                order.ShippingAddress.AddressLine1,
                order.ShippingAddress.AddressLine2 ?? string.Empty,
                order.ShippingAddress.City,
                order.ShippingAddress.State,
                order.ShippingAddress.PostCode,
                order.ShippingAddress.Country));
}