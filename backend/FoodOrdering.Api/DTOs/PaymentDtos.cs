using System.ComponentModel.DataAnnotations;
using FoodOrdering.Api.Models;

namespace FoodOrdering.Api.DTOs;

public class CheckoutWithPaymentRequest
{
    [Required, MinLength(5), MaxLength(300)]
    public string DeliveryAddress { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Notes { get; set; }

    [Required, MinLength(1)]
    public List<OrderItemRequest> Items { get; set; } = new();

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    public CardPaymentDetails? Card { get; set; }
    public VodafonePaymentDetails? Vodafone { get; set; }
    public string? FawrySessionId { get; set; }
}

public class CardPaymentDetails
{
    [Required, MinLength(13), MaxLength(19)]
    public string CardNumber { get; set; } = string.Empty;

    [Required, RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$")]
    public string Expiry { get; set; } = string.Empty;

    [Required, MinLength(3), MaxLength(4)]
    public string Cvv { get; set; } = string.Empty;

    [Required, MinLength(2), MaxLength(100)]
    public string CardholderName { get; set; } = string.Empty;
}

public class VodafonePaymentDetails
{
    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(6)]
    public string Otp { get; set; } = string.Empty;

    public string? OtpSessionId { get; set; }
}

public class VodafoneOtpRequest
{
    [Required]
    public string Phone { get; set; } = string.Empty;

    [Range(0.01, 100000)]
    public decimal Amount { get; set; }
}

public class VodafoneOtpResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string MaskedPhone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int ExpiresInSeconds { get; set; }
}

public class FawryInitiateRequest
{
    [Range(0.01, 100000)]
    public decimal Amount { get; set; }
}

public class FawryInitiateResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string FawryCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string Instructions { get; set; } = string.Empty;
}

public class PaymentProcessResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public OrderDto? Order { get; set; }
    public int ProcessingTimeMs { get; set; }
}

public class DemoPaymentInfoResponse
{
    public List<DemoCardInfo> TestCards { get; set; } = new();
    public string VodafoneDemoOtp { get; set; } = "123456";
    public string Note { get; set; } = "Demo mode — no real money is charged.";
}

public class DemoCardInfo
{
    public string Number { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
}
