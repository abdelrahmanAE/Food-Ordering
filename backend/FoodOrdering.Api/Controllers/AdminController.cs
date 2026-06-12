using FoodOrdering.Api.Data;
using FoodOrdering.Api.DTOs;
using FoodOrdering.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db) => _db = db;

    [HttpGet("orders")]
    public async Task<ActionResult<List<AdminOrderDto>>> GetAllOrders()
    {
        var orders = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new AdminOrderDto
            {
                Id = o.Id,
                CustomerName = o.User.FullName,
                CustomerEmail = o.User.Email,
                Status = o.Status.ToString(),
                TotalAmount = o.TotalAmount,
                PaymentMethod = o.PaymentMethod.ToString(),
                DeliveryAddress = o.DeliveryAddress,
                CreatedAt = o.CreatedAt,
                ItemCount = o.Items.Sum(i => i.Quantity)
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpPatch("orders/{id:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order is null) return NotFound();

        order.Status = request.Status;
        order.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new { message = "Order status updated.", status = order.Status.ToString() });
    }

    [HttpPost("menu")]
    public async Task<ActionResult<MenuItemDto>> CreateMenuItem([FromBody] CreateMenuItemRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var categoryExists = await _db.Categories.AnyAsync(c => c.Id == request.CategoryId);
        if (!categoryExists) return BadRequest(new { message = "Invalid category." });

        var item = new MenuItem
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Price = request.Price,
            ImageUrl = request.ImageUrl.Trim(),
            CategoryId = request.CategoryId,
            RestaurantId = request.RestaurantId,
            IsAvailable = request.IsAvailable,
            IsFeatured = request.IsFeatured
        };

        _db.MenuItems.Add(item);
        await _db.SaveChangesAsync();

        var category = await _db.Categories.FindAsync(item.CategoryId);
        return CreatedAtAction(nameof(GetMenuItem), new { id = item.Id }, new MenuItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            ImageUrl = item.ImageUrl,
            IsAvailable = item.IsAvailable,
            IsFeatured = item.IsFeatured,
            CategoryId = item.CategoryId,
            CategoryName = category!.Name
        });
    }

    [HttpGet("menu/{id:int}")]
    public async Task<ActionResult<MenuItemDto>> GetMenuItem(int id)
    {
        var item = await _db.MenuItems.Include(m => m.Category).FirstOrDefaultAsync(m => m.Id == id);
        if (item is null) return NotFound();

        return Ok(new MenuItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            ImageUrl = item.ImageUrl,
            IsAvailable = item.IsAvailable,
            IsFeatured = item.IsFeatured,
            CategoryId = item.CategoryId,
            CategoryName = item.Category.Name
        });
    }

    [HttpPut("menu/{id:int}")]
    public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] UpdateMenuItemRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var item = await _db.MenuItems.FindAsync(id);
        if (item is null) return NotFound();

        item.Name = request.Name.Trim();
        item.Description = request.Description.Trim();
        item.Price = request.Price;
        item.ImageUrl = request.ImageUrl.Trim();
        item.CategoryId = request.CategoryId;
        item.IsAvailable = request.IsAvailable;
        item.IsFeatured = request.IsFeatured;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("menu/{id:int}")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        var item = await _db.MenuItems.FindAsync(id);
        if (item is null) return NotFound();

        item.IsAvailable = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

public class AdminOrderDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int ItemCount { get; set; }
}
