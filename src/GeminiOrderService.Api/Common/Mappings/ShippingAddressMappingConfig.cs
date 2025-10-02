using GeminiOrderService.Application.Orders.Commands;
using Mapster;

namespace GeminiOrderService.Api.Common.Mappings;

public sealed class ShippingAddressMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Contracts.Orders.ShippingAddressRequest, ShippingAddressCommand>()
            .Map(dest => dest, src => src);
    }
}