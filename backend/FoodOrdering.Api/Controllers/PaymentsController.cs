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
public class PaymentsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly DemoPaymentService _payments;

    public PaymentsController(AppDbContext db, DemoPaymentService payments)
    {
        _db = db;
        _payments = payments;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("demo-info")]
    public ActionResult<DemoPaymentInfoResponse> GetDemoInfo() =>
        Ok(_payments.GetDemoInfo());

    [HttpPost("vodafone/request-otp")]
    [Authorize]
    public ActionResult<VodafoneOtpResponse> RequestVodafoneOtp([FromBody] VodafoneOtpRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            return Ok(_payments.RequestVodafoneOtp(request, GetUserId()));
        }
        catch (PaymentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("fawry/initiate")]
    [Authorize]
    public ActionResult<FawryInitiateResponse> InitiateFawry([FromBody] FawryInitiateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(_payments.InitiateFawry(request, GetUserId()));
    }

    [HttpPost("fawry/simulate-pay/{sessionId}")]
    [Authorize]
    public IActionResult SimulateFawryPayment(string sessionId)
    {
        try
        {
            _payments.MarkFawryPaid(sessionId, GetUserId());
            return Ok(new { message = "Fawry payment simulated successfully.", paid = true });
        }
        catch (PaymentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("checkout")]
    [Authorize]
    public async Task<ActionResult<PaymentProcessResponse>> CheckoutWithPayment(
        [FromBody] CheckoutWithPaymentRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var sw = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            var (restaurant, orderItems, total) = await BuildOrderDataAsync(request);

            await Task.Delay(request.PaymentMethod == PaymentMethod.Visa ? 1800 : 600);

            var paymentResult = _payments.Process(request, GetUserId(), total);

            if (!paymentResult.IsSuccess)
            {
                sw.Stop();
                return BadRequest(new PaymentProcessResponse
                {
                    Success = false,
                    Message = paymentResult.Message,
                    ProcessingTimeMs = (int)sw.ElapsedMilliseconds
                });
            }

            var order = new Order
            {
                UserId = GetUserId(),
                DeliveryAddress = request.DeliveryAddress.Trim(),
                Notes = request.Notes?.Trim(),
                TotalAmount = total,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = paymentResult.PayOnDelivery ? PaymentStatus.PayOnDelivery : PaymentStatus.Paid,
                PaymentReference = paymentResult.PaymentReference,
                TransactionId = paymentResult.TransactionId,
                PaidAt = paymentResult.PayOnDelivery ? null : DateTime.UtcNow,
                Status = OrderStatus.Confirmed,
                Items = orderItems
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            sw.Stop();

            return Ok(new PaymentProcessResponse
            {
                Success = true,
                Message = paymentResult.Message,
                ProcessingTimeMs = (int)sw.ElapsedMilliseconds,
                Order = await MapOrderAsync(order.Id)
            });
        }
        catch (PaymentException ex)
        {
            sw.Stop();
            return BadRequest(new PaymentProcessResponse
            {
                Success = false,
                Message = ex.Message,
                ProcessingTimeMs = (int)sw.ElapsedMilliseconds
            });
        }
    }

    private async Task<(Restaurant restaurant, List<OrderItem> items, decimal total)> BuildOrderDataAsync(
        CheckoutWithPaymentRequest request)
    {
        var menuItemIds = request.Items.Select(i => i.MenuItemId).Distinct().ToList();
        var menuItems = await _db.MenuItems
            .Include(m => m.Restaurant)
            .Where(m => menuItemIds.Contains(m.Id) && m.IsAvailable)
            .ToListAsync();

        if (menuItems.Count != menuItemIds.Count)
            throw new PaymentException("One or more items are unavailable.");

        var restaurantIds = menuItems.Select(m => m.RestaurantId).Distinct().ToList();
        if (restaurantIds.Count > 1)
            throw new PaymentException("All items must be from the same restaurant.");

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
            throw new PaymentException($"Minimum order for {restaurant.Name} is {restaurant.MinOrder:0} EGP.");

        return (restaurant, orderItems, subtotal + restaurant.DeliveryFee);
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
            DeliveryAddress = order.DeliveryAddress,
            Notes = order.Notes,
            CreatedAt = order.CreatedAt,
            PaidAt = order.PaidAt,
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
        PaymentMethod.Visa => "Visa / Mastercard",
        PaymentMethod.Fawry => "Fawry",
        PaymentMethod.VodafoneCash => "Vodafone Cash",
        _ => method.ToString()
    };
}
