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