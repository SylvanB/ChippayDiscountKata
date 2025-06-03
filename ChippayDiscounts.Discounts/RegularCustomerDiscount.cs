using ChippayDiscounts.Core;

namespace ChippayDiscounts.Discounts;

public class RegularCustomerDiscount : IDiscountStrategy
{
    public decimal GetDiscount(IList<BasketItem> basket) => 0m;
}