using Mapster;
using GeminiOrderService.Application.Common.Models.Orders;
using GeminiOrderService.Contracts.Orders;
using GeminiOrderService.Application.Orders.Commands;

namespace GeminiOrderService.Api.Common.Mappings;

public class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<OrderItemModel, OrderItemResponse>()
            .Map(dest => dest.Id, src => src.ProductId)
            .Map(dest => dest, src => src);

        config.NewConfig<OrderModel, OrderResponse>()
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.Items, src => src.Items.Select(item => item.Adapt<OrderItemResponse>()))
            .Map(dest => dest, src => src);
    }
}

public static class OrderMappingExtensions
{
    public static CreateOrderCommand ToCreateOrderCommand(this CreateOrderRequest model) =>
        new CreateOrderCommand(
            model.CustomerId ?? Guid.Empty,
            model.Currency,
            new ShippingAddressCommand(
                model.FirstName,
                model.LastName,
                model.ShippingAddress.AddressLine1,
                model.ShippingAddress.AddressLine2,
                model.ShippingAddress.City,
                model.ShippingAddress.State,
                model.ShippingAddress.PostCode,
                model.ShippingAddress.Country
            ),
            model.Items.Select(item => new CreateOrderItemCommand(
                item.ProductId,
                item.Quantity,
                item.UnitPrice)));
}