namespace ChippayDiscounts.Core;

public class DiscountCalculator(IDiscountStrategy strategy)
{
    
    /// <summary>
    /// Apply the discount to the basket subtotal
    /// </summary>
    /// <param name="basket">A collection of <see cref="BasketItem"/> that represents the shoppers basket</param>
    /// <returns>The total, minus any applied discounts</returns>
    public decimal ApplyDiscount(IList<BasketItem> basket)
    {
        var total = basket.Sum(i => i.Price);
        return total - strategy.GetDiscount(basket);
    }
}
