using System.ComponentModel.DataAnnotations;
using FoodOrdering.Api.Models;

namespace FoodOrdering.Api.DTOs;

public class OrderItemRequest
{
    [Required]
    public int MenuItemId { get; set; }

    [Range(1, 50)]
    public int Quantity { get; set; }
}

public class CreateOrderRequest
{
    [Required, MinLength(5), MaxLength(300)]
    public string DeliveryAddress { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Notes { get; set; }

    [Required, MinLength(1)]
    public List<OrderItemRequest> Items { get; set; } = new();

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    [MaxLength(20)]
    public string? PaymentPhone { get; set; }
}

public class OrderItemDto
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}

public class OrderDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string? PaymentReference { get; set; }
    public string? TransactionId { get; set; }
    public DateTime? PaidAt { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class UpdateOrderStatusRequest
{
    [Required]
    public OrderStatus Status { get; set; }
}
