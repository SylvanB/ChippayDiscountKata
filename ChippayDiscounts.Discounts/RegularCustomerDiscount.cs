using ChippayDiscounts.Core;

namespace ChippayDiscounts.Discounts;

/// <summary>
/// A discount for loyal Chippay customers.
/// </summary>
public class RegularCustomerDiscount : IDiscountStrategy
{
    /// <summary>
    /// Get the discount to be applied to the user's basket.
    /// </summary>
    /// <param name="basket">The items the user has in their basket.</param>
    /// <returns>The amount to be subtracted from the basket total.</returns>
    public decimal GetDiscount(IList<BasketItem> basket) => 0m;
}