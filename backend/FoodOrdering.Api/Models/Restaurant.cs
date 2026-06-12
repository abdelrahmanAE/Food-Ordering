namespace FoodOrdering.Api.Models;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string LogoEmoji { get; set; } = "🍽️";
    public string BrandColor { get; set; } = "#e85d04";
    public string DeliveryTime { get; set; } = "30-45 min";
    public decimal DeliveryFee { get; set; } = 25m;
    public decimal MinOrder { get; set; } = 50m;
    public double Rating { get; set; } = 4.5;
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }

    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
