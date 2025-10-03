using GeminiOrderService.Domain.Common.Models;
using GeminiOrderService.Domain.Orders.Entities;

namespace GeminiOrderService.Domain.Orders.Events;

public sealed record OrderSubmitted(Order Order, IEnumerable<OrderItem> OrderItems) : IDomainEvent;