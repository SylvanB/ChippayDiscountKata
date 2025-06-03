namespace ChippayDiscounts.Core;

public interface IDiscountStrategy
{
    decimal GetDiscount(IList<BasketItem> basket);
}