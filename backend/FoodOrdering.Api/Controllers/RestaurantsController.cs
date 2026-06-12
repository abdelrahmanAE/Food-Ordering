using FoodOrdering.Api.Data;
using FoodOrdering.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController : ControllerBase
{
    private readonly AppDbContext _db;

    public RestaurantsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<RestaurantDto>>> GetAll()
    {
        var restaurants = await _db.Restaurants
            .Where(r => r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => new RestaurantDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                ImageUrl = r.ImageUrl,
                LogoEmoji = r.LogoEmoji,
                BrandColor = r.BrandColor,
                DeliveryTime = r.DeliveryTime,
                DeliveryFee = r.DeliveryFee,
                MinOrder = r.MinOrder,
                Rating = r.Rating,
                MenuCount = r.MenuItems.Count(m => m.IsAvailable)
            })
            .ToListAsync();

        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RestaurantDto>> GetById(int id)
    {
        var restaurant = await _db.Restaurants
            .Where(r => r.Id == id && r.IsActive)
            .Select(r => new RestaurantDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                ImageUrl = r.ImageUrl,
                LogoEmoji = r.LogoEmoji,
                BrandColor = r.BrandColor,
                DeliveryTime = r.DeliveryTime,
                DeliveryFee = r.DeliveryFee,
                MinOrder = r.MinOrder,
                Rating = r.Rating,
                MenuCount = r.MenuItems.Count(m => m.IsAvailable)
            })
            .FirstOrDefaultAsync();

        if (restaurant is null) return NotFound();
        return Ok(restaurant);
    }
}
