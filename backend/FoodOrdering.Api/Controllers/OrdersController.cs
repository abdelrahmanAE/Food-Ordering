using System.Security.Claims;
using FoodOrdering.Api.Data;
using FoodOrdering.Api.DTOs;
using FoodOrdering.Api.Models;
using FoodOrdering.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly DemoPaymentService _paymentService;

    public OrdersController(AppDbContext db, DemoPaymentService paymentService)
    {
        _db = db;
        _paymentService = paymentService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CheckoutWithPaymentRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var paymentError = ValidatePayment(request);
        if (paymentError is not null)
            return BadRequest(new { message = paymentError });

        var menuItemIds = request.Items.Select(i => i.MenuItemId).Distinct().ToList();
        var menuItems = await _db.MenuItems
            .Include(m => m.Restaurant)
            .Where(m => menuItemIds.Contains(m.Id) && m.IsAvailable)
            .ToListAsync();

        if (menuItems.Count != menuItemIds.Count)
            return BadRequest(new { message = "One or more items are unavailable." });

        var restaurantIds = menuItems.Select(m => m.RestaurantId).Distinct().ToList();
        if (restaurantIds.Count > 1)
            return BadRequest(new { message = "All items must be from the same restaurant." });

        var restaurant = menuItems[0].Restaurant;
        var orderItems = new List<OrderItem>();
        decimal subtotal = 0;

        foreach (var item in request.Items)
        {
            var menuItem = menuItems.First(m => m.Id == item.MenuItemId);
            orderItems.Add(new OrderItem
            {
                MenuItemId = menuItem.Id,
                Quantity = item.Quantity,
                UnitPrice = menuItem.Price
            });
            subtotal += menuItem.Price * item.Quantity;
        }

        if (subtotal < restaurant.MinOrder)
            return BadRequest(new { message = $"Minimum order for {restaurant.Name} is {restaurant.MinOrder:0} EGP." });

        var total = subtotal + restaurant.DeliveryFee;
        var paymentRef = BuildPaymentReference(request);

        // Process payment via the Service (validates card, OTP, or Fawry session)
        PaymentResult paymentResult;
        try
        {
            paymentResult = _paymentService.Process(request, GetUserId(), total);
            if (!paymentResult.IsSuccess)
                return BadRequest(new { message = paymentResult.Message });
        }
        catch (PaymentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }

        var order = new Order
        {
            UserId = GetUserId(),
            DeliveryAddress = request.DeliveryAddress.Trim(),
            Notes = request.Notes?.Trim(),
            TotalAmount = total,
            PaymentMethod = request.PaymentMethod,
            PaymentReference = paymentResult.PaymentReference,
            TransactionId = paymentResult.TransactionId,
            PaymentStatus = paymentResult.PayOnDelivery ? PaymentStatus.PayOnDelivery : PaymentStatus.Paid,
            PaidAt = paymentResult.PayOnDelivery ? null : DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Items = orderItems
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, await MapOrderAsync(order.Id));
    }

    private static string? ValidatePayment(CheckoutWithPaymentRequest request)
    {
        return request.PaymentMethod switch
        {
            PaymentMethod.VodafoneCash when request.Vodafone is null || string.IsNullOrWhiteSpace(request.Vodafone.Phone)
                => "Vodafone Cash requires your phone number.",
            PaymentMethod.VodafoneCash when request.Vodafone is not null && !IsValidEgyptPhone(request.Vodafone.Phone)
                => "Enter a valid Egyptian phone number (e.g. 01012345678).",
            PaymentMethod.Visa when request.Card is null
                => "Visa payment requires card details.",
            PaymentMethod.Fawry when string.IsNullOrWhiteSpace(request.FawrySessionId)
                => "Fawry payment requires a session id.",
            _ => null
        };
    }

    private static bool IsValidEgyptPhone(string phone)
    {
        var digits = new string(phone.Where(char.IsDigit).ToArray());
        return digits.Length is 10 or 11 && digits.StartsWith("01");
    }

    private static string? BuildPaymentReference(CheckoutWithPaymentRequest request)
    {
        return request.PaymentMethod switch
        {
            PaymentMethod.Fawry => $"FAW-{Random.Shared.Next(100000, 999999)}",
            PaymentMethod.VodafoneCash => request.Vodafone?.Phone?.Trim(),
            PaymentMethod.Visa => $"VISA-****{Random.Shared.Next(1000, 9999)}",
            _ => null
        };
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
    {
        var userId = GetUserId();
        var orderIds = await _db.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => o.Id)
            .ToListAsync();

        var result = new List<OrderDto>();
        foreach (var id in orderIds)
            result.Add(await MapOrderAsync(id));

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var userId = GetUserId();
        var isAdmin = User.IsInRole("Admin");

        var exists = await _db.Orders.AnyAsync(o => o.Id == id && (o.UserId == userId || isAdmin));
        if (!exists) return NotFound();

        return Ok(await MapOrderAsync(id));
    }

    private async Task<OrderDto> MapOrderAsync(int orderId)
    {
        var order = await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .FirstAsync(o => o.Id == orderId);

        return new OrderDto
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            PaymentMethod = FormatPaymentMethod(order.PaymentMethod),
            PaymentStatus = order.PaymentStatus.ToString(),
            PaymentReference = order.PaymentReference,
            TransactionId = order.TransactionId,
            PaidAt = order.PaidAt,
            DeliveryAddress = order.DeliveryAddress,
            Notes = order.Notes,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(i => new OrderItemDto
            {
                MenuItemId = i.MenuItemId,
                Name = i.MenuItem.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }

    private static string FormatPaymentMethod(PaymentMethod method) => method switch
    {
        PaymentMethod.CashOnDelivery => "Cash on Delivery",
        PaymentMethod.Visa => "Visa",
        PaymentMethod.Fawry => "Fawry",
        PaymentMethod.VodafoneCash => "Vodafone Cash",
        _ => method.ToString()
    };
}
