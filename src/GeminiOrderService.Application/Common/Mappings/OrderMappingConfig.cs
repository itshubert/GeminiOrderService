using GeminiOrderService.Application.Common.Models.Orders;
using GeminiOrderService.Domain.Orders;
using GeminiOrderService.Domain.Orders.Entities;
using Mapster;

namespace GeminiOrderService.Application.Common.Mappings;

public class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderItem, OrderItemModel>()
            .Map(dest => dest.ProductName, src => src.ProductNameSnapshot)
            .Map(dest => dest, src => src);

        config.NewConfig<Order, OrderModel>()
            .Map(dest => dest.Items, src => src.Items.Select(item => item.Adapt<OrderItemModel>()));
    }
}