using System.ComponentModel.DataAnnotations;

namespace FoodOrdering.Api.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int RestaurantId { get; set; }
}

public class MenuItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public bool IsFeatured { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
}

public class CreateMenuItemRequest
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 10000)]
    public decimal Price { get; set; }

    [Required, Url]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int RestaurantId { get; set; }

    public bool IsAvailable { get; set; } = true;
    public bool IsFeatured { get; set; }
}

public class UpdateMenuItemRequest : CreateMenuItemRequest { }
