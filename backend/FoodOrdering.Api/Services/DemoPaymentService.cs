using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using FoodOrdering.Api.DTOs;
using FoodOrdering.Api.Models;

namespace FoodOrdering.Api.Services;

public class DemoPaymentService
{
    private const string DemoOtp = "123456";
    private const string SuccessCard = "4242424242424242";
    private const string DeclineCard = "4000000000000002";
    private const string InsufficientCard = "4000000000009995";

    private readonly ConcurrentDictionary<string, OtpSession> _otpSessions = new();
    private readonly ConcurrentDictionary<string, FawrySession> _fawrySessions = new();

    public DemoPaymentInfoResponse GetDemoInfo() => new()
    {
        Note = "Demo sandbox — no real charges. Practice like production.",
        VodafoneDemoOtp = DemoOtp,
        TestCards = new List<DemoCardInfo>
        {
            new() { Number = "4242 4242 4242 4242", Result = "Payment succeeds" },
            new() { Number = "4000 0000 0000 0002", Result = "Card declined" },
            new() { Number = "4000 0000 0000 9995", Result = "Insufficient funds" }
        }
    };

    public VodafoneOtpResponse RequestVodafoneOtp(VodafoneOtpRequest request, int userId)
    {
        var digits = NormalizePhone(request.Phone);
        if (!IsValidEgyptPhone(digits))
            throw new PaymentException("Enter a valid Vodafone number (010, 011, 012, 015).");

        var sessionId = Guid.NewGuid().ToString("N");
        _otpSessions[sessionId] = new OtpSession
        {
            UserId = userId,
            Phone = digits,
            Amount = request.Amount,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };

        return new VodafoneOtpResponse
        {
            SessionId = sessionId,
            MaskedPhone = MaskPhone(digits),
            Message = $"Demo USSD push sent to {MaskPhone(digits)}. Enter OTP to confirm.",
            ExpiresInSeconds = 300
        };
    }

    public FawryInitiateResponse InitiateFawry(FawryInitiateRequest request, int userId)
    {
        var code = $"9{Random.Shared.Next(100000000, 999999999)}";
        var sessionId = Guid.NewGuid().ToString("N");

        _fawrySessions[sessionId] = new FawrySession
        {
            UserId = userId,
            Code = code,
            Amount = request.Amount,
            ExpiresAt = DateTime.UtcNow.AddHours(48),
            IsPaid = false
        };

        return new FawryInitiateResponse
        {
            SessionId = sessionId,
            FawryCode = code,
            Amount = request.Amount,
            ExpiresAt = DateTime.UtcNow.AddHours(48),
            Instructions = "Go to any Fawry outlet or use Fawry app → Pay → enter code above."
        };
    }

    public void MarkFawryPaid(string sessionId, int userId)
    {
        if (!_fawrySessions.TryGetValue(sessionId, out var session))
            throw new PaymentException("Fawry session expired. Please generate a new code.");

        if (session.UserId != userId)
            throw new PaymentException("Invalid Fawry session.");

        if (session.ExpiresAt < DateTime.UtcNow)
        {
            _fawrySessions.TryRemove(sessionId, out _);
            throw new PaymentException("Fawry code expired. Generate a new one.");
        }

        session.IsPaid = true;
    }

    public PaymentResult Process(CheckoutWithPaymentRequest request, int userId, decimal total)
    {
        return request.PaymentMethod switch
        {
            PaymentMethod.CashOnDelivery => PaymentResult.Success(
                null, PayOnDelivery: true, "Order confirmed. Pay cash on delivery."),

            PaymentMethod.Visa => ProcessCard(request.Card!, total),

            PaymentMethod.VodafoneCash => ProcessVodafone(request.Vodafone!, userId, total),

            PaymentMethod.Fawry => ProcessFawry(request.FawrySessionId!, userId, total),

            _ => throw new PaymentException("Unsupported payment method.")
        };
    }

    private PaymentResult ProcessCard(CardPaymentDetails card, decimal total)
    {
        if (card is null) throw new PaymentException("Card details are required.");

        var number = NormalizeCard(card.CardNumber);

        if (number.Length < 13 || number.Length > 19)
            throw new PaymentException("Invalid card number.");

        if (!IsValidExpiry(card.Expiry))
            throw new PaymentException("Card has expired or invalid expiry (use MM/YY).");

        if (!Regex.IsMatch(card.Cvv, @"^\d{3,4}$"))
            throw new PaymentException("Invalid CVV.");

        if (string.IsNullOrWhiteSpace(card.CardholderName))
            throw new PaymentException("Cardholder name is required.");

        return number switch
        {
            DeclineCard => PaymentResult.Failed("Your card was declined. Try another card."),
            InsufficientCard => PaymentResult.Failed("Insufficient funds on this card."),
            SuccessCard => PaymentResult.Success(
                $"TXN-{Guid.NewGuid().ToString("N")[..12].ToUpper()}",
                Message: $"Paid {total:0} EGP with Visa •••• 4242"),
            _ when PassesLuhn(number) => PaymentResult.Success(
                $"TXN-{Guid.NewGuid().ToString("N")[..12].ToUpper()}",
                Message: $"Paid {total:0} EGP with card •••• {number[^4..]}"),
            _ => PaymentResult.Failed("Invalid card number. Use demo card 4242 4242 4242 4242.")
        };
    }

    private PaymentResult ProcessVodafone(VodafonePaymentDetails vodafone, int userId, decimal total)
    {
        if (vodafone is null) throw new PaymentException("Vodafone Cash details are required.");

        if (string.IsNullOrWhiteSpace(vodafone.OtpSessionId))
            throw new PaymentException("Request OTP first before paying.");

        if (!_otpSessions.TryGetValue(vodafone.OtpSessionId, out var session))
            throw new PaymentException("OTP session expired. Request a new code.");

        if (session.UserId != userId)
            throw new PaymentException("Invalid OTP session.");

        if (session.ExpiresAt < DateTime.UtcNow)
        {
            _otpSessions.TryRemove(vodafone.OtpSessionId, out _);
            throw new PaymentException("OTP expired. Request a new one.");
        }

        var phone = NormalizePhone(vodafone.Phone);
        if (phone != session.Phone)
            throw new PaymentException("Phone number does not match OTP session.");

        if (vodafone.Otp.Trim() != DemoOtp)
            throw new PaymentException("Invalid OTP. Demo OTP is 123456.");

        _otpSessions.TryRemove(vodafone.OtpSessionId, out _);

        return PaymentResult.Success(
            $"VF-{Guid.NewGuid().ToString("N")[..10].ToUpper()}",
            Message: $"Paid {total:0} EGP via Vodafone Cash ({MaskPhone(phone)})");
    }

    private PaymentResult ProcessFawry(string sessionId, int userId, decimal total)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
            throw new PaymentException("Complete Fawry payment first.");

        if (!_fawrySessions.TryGetValue(sessionId, out var session))
            throw new PaymentException("Fawry session not found.");

        if (session.UserId != userId)
            throw new PaymentException("Invalid Fawry session.");

        if (!session.IsPaid)
            throw new PaymentException("Fawry payment not confirmed yet. Pay at outlet or simulate payment.");

        if (Math.Abs(session.Amount - total) > 0.01m)
            throw new PaymentException("Order total changed. Generate a new Fawry code.");

        _fawrySessions.TryRemove(sessionId, out _);

        return PaymentResult.Success(
            session.Code,
            Message: $"Fawry payment confirmed — {total:0} EGP");
    }

    private static bool IsValidExpiry(string expiry)
    {
        var parts = expiry.Split('/');
        if (parts.Length != 2) return false;
        if (!int.TryParse(parts[0], out var month) || month is < 1 or > 12) return false;
        if (!int.TryParse(parts[1], out var year)) return false;
        year += 2000;
        var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        return lastDay >= DateTime.UtcNow.Date;
    }

    private static bool PassesLuhn(string number)
    {
        var sum = 0;
        var alt = false;
        for (var i = number.Length - 1; i >= 0; i--)
        {
            var n = number[i] - '0';
            if (alt) { n *= 2; if (n > 9) n -= 9; }
            sum += n;
            alt = !alt;
        }
        return sum % 10 == 0;
    }

    private static string NormalizeCard(string card) =>
        new string(card.Where(char.IsDigit).ToArray());

    private static string NormalizePhone(string phone) =>
        new string(phone.Where(char.IsDigit).ToArray());

    private static bool IsValidEgyptPhone(string digits) =>
        digits.Length is 10 or 11 && digits.StartsWith("01");

    private static string MaskPhone(string digits)
    {
        if (digits.Length < 7) return "***";
        return $"{digits[..3]}***{digits[^4..]}";
    }

    private class OtpSession
    {
        public int UserId { get; set; }
        public string Phone { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    private class FawrySession
    {
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsPaid { get; set; }
    }
}

public class PaymentResult
{
    public bool IsSuccess { get; init; }
    public bool PayOnDelivery { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? TransactionId { get; init; }
    public string? PaymentReference { get; init; }

    public static PaymentResult Success(string? txnId, bool PayOnDelivery = false, string Message = "") =>
        new() { IsSuccess = true, PayOnDelivery = PayOnDelivery, TransactionId = txnId, PaymentReference = txnId, Message = Message };

    public static PaymentResult Failed(string message) =>
        new() { IsSuccess = false, Message = message };
}

public class PaymentException : Exception
{
    public PaymentException(string message) : base(message) { }
}
