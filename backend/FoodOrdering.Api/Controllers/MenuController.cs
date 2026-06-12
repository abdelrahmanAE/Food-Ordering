using FoodOrdering.Api.Data;
using FoodOrdering.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly AppDbContext _db;

    public MenuController(AppDbContext db) => _db = db;

    [HttpGet("categories")]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories([FromQuery] int? restaurantId)
    {
        var query = _db.Categories.AsQueryable();
        if (restaurantId.HasValue)
            query = query.Where(c => c.RestaurantId == restaurantId.Value);

        var categories = await query
            .OrderBy(c => c.SortOrder)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                RestaurantId = c.RestaurantId
            })
            .ToListAsync();

        return Ok(categories);
    }

    [HttpGet]
    public async Task<ActionResult<List<MenuItemDto>>> GetMenu(
        [FromQuery] int? restaurantId,
        [FromQuery] int? categoryId,
        [FromQuery] string? search)
    {
        var query = _db.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Restaurant)
            .Where(m => m.IsAvailable)
            .AsQueryable();

        if (restaurantId.HasValue)
            query = query.Where(m => m.RestaurantId == restaurantId.Value);

        if (categoryId.HasValue)
            query = query.Where(m => m.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(m =>
                m.Name.ToLower().Contains(term) ||
                m.Description.ToLower().Contains(term));
        }

        var items = await query
            .OrderBy(m => m.Category.SortOrder)
            .ThenBy(m => m.Name)
            .Select(m => new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsAvailable = m.IsAvailable,
                IsFeatured = m.IsFeatured,
                CategoryId = m.CategoryId,
                CategoryName = m.Category.Name,
                RestaurantId = m.RestaurantId,
                RestaurantName = m.Restaurant.Name
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("featured")]
    public async Task<ActionResult<List<MenuItemDto>>> GetFeatured([FromQuery] int? restaurantId)
    {
        var query = _db.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Restaurant)
            .Where(m => m.IsAvailable && m.IsFeatured);

        if (restaurantId.HasValue)
            query = query.Where(m => m.RestaurantId == restaurantId.Value);

        var items = await query
            .Select(m => new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsAvailable = m.IsAvailable,
                IsFeatured = m.IsFeatured,
                CategoryId = m.CategoryId,
                CategoryName = m.Category.Name,
                RestaurantId = m.RestaurantId,
                RestaurantName = m.Restaurant.Name
            })
            .Take(8)
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MenuItemDto>> GetById(int id)
    {
        var item = await _db.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Restaurant)
            .Where(m => m.Id == id)
            .Select(m => new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsAvailable = m.IsAvailable,
                IsFeatured = m.IsFeatured,
                CategoryId = m.CategoryId,
                CategoryName = m.Category.Name,
                RestaurantId = m.RestaurantId,
                RestaurantName = m.Restaurant.Name
            })
            .FirstOrDefaultAsync();

        if (item is null) return NotFound();
        return Ok(item);
    }
}
