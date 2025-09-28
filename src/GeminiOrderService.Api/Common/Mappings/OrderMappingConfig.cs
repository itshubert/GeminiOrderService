using Mapster;
using GeminiOrderService.Application.Common.Models.Orders;
using GeminiOrderService.Contracts.Orders;

namespace GeminiOrderService.Api.Common.Mappings;

public class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderItemModel, OrderItemResponse>()
            .Map(dest => dest.Id, src => src.ProductId)
            .Map(dest => dest, src => src);

        config.NewConfig<OrderModel, OrderResponse>()
            .Map(dest => dest.CustomerName, src => $"Customer-{src.CustomerId}") // Placeholder mapping - you may want to fetch actual customer name
            .Map(dest => dest.Items, src => src.Items.Select(item => item.Adapt<OrderItemResponse>()));
    }
}