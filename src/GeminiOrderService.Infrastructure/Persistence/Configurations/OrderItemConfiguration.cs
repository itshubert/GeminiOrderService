using GeminiOrderService.Domain.Common.Models;
using GeminiOrderService.Domain.Orders.Entities;
using GeminiOrderService.Domain.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeminiOrderService.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderItemId.Create(value)
            );

        builder.Property(oi => oi.ProductId)
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        // Configure the foreign key to Order with proper conversion
        builder.Property<OrderId>("OrderId")
            .IsRequired()
            .HasConversion(
                orderId => orderId.Value,
                value => OrderId.Create(value)
            );

        builder.Property(oi => oi.ProductNameSnapshot)
            .IsRequired()
            .HasMaxLength(200);

        // Configure UnitPrice value object
        builder.Property(oi => oi.UnitPrice)
            .IsRequired()
            .HasConversion(
                up => up.Value,
                value => Price.Create(value).Value);


        // Configure SubTotal value object
        builder.Property(oi => oi.SubTotal)
            .IsRequired()
            .HasConversion(
                st => st.Value,
                value => Price.Create(value).Value);
    }
}