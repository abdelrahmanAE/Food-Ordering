namespace FoodOrdering.Api.DTOs;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string LogoEmoji { get; set; } = string.Empty;
    public string BrandColor { get; set; } = string.Empty;
    public string DeliveryTime { get; set; } = string.Empty;
    public decimal DeliveryFee { get; set; }
    public decimal MinOrder { get; set; }
    public double Rating { get; set; }
    public int MenuCount { get; set; }
}
