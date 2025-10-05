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
            .HasConversion<string>()
            .HasMaxLength(50);

        // Add CHECK constraint to ensure only valid OrderStatus values
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Orders_Status",
            "\"Status\" IN ('Pending', 'Confirmed', 'Shipped', 'Delivered', 'Cancelled')"));

        builder.Property(o => o.Currency)
            .IsRequired()
            .HasMaxLength(3); // ISO currency code (e.g., USD, EUR)

        // Configure TotalAmount value object
        builder.Property(o => o.TotalAmount)
            .IsRequired()
            .HasConversion(
                ta => ta.Value,
                value => Price.Create(value).Value);

        // Configure ShippingAddress as an owned entity type (columns in Orders table)
        builder.OwnsOne(o => o.ShippingAddress, sa =>
        {
            sa.Property(a => a.AddressLine1)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("AddressLine1");

            sa.Property(a => a.AddressLine2)
                .HasMaxLength(200)
                .HasColumnName("AddressLine2");

            sa.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("City");

            sa.Property(a => a.State)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("State");

            sa.Property(a => a.PostCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("PostCode");

            sa.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Country");
        });

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