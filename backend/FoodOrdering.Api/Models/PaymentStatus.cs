namespace FoodOrdering.Api.Models;

public enum PaymentStatus
{
    AwaitingPayment = 0,
    Paid = 1,
    Failed = 2,
    PayOnDelivery = 3
}
