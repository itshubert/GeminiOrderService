using GeminiOrderService.Domain.Common.Models;
using GeminiOrderService.Domain.Orders;
using GeminiOrderService.Domain.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeminiOrderService.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value)
            );

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.Currency)
            .IsRequired()
            .HasMaxLength(3); // ISO currency code (e.g., USD, EUR)

        // Configure TotalAmount value object
        builder.Property(o => o.TotalAmount)
            .IsRequired()
            .HasConversion(
                ta => ta.Value,
                value => Price.Create(value).Value);

        // Configure the one-to-many relationship with OrderItems
        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .HasPrincipalKey(o => o.Id)
            .OnDelete(DeleteBehavior.NoAction);

        // Configure the backing field for OrderItems
        builder.Navigation(o => o.Items)
            .HasField("_orderItems")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}