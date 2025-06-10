using System.Reflection;
using ChippayDiscounts.Discounts;
using ChippayDiscounts.Core;

namespace ChippayDiscounts.Tests;

public class WhenUsingTheDiscountCalculator : DiscountTestsBase
{
    [Test]
    public void ThenRegularDiscountAppliesFixedPercentage()
    {
        var basketItems = new[]
        {
            new BasketItem("Apples", 50m),
            new BasketItem("Oranges", 50m)
        };

        var strategy = new RegularCustomerDiscount();
        var calc = new DiscountCalculator(strategy);

        var result = calc.ApplyDiscount(basketItems);

        Assert.That(result, Is.EqualTo(95m));
    }
    
    [Test, Ignore("implement VIP discount logic")]
    public void ThenVipDiscountAppliesFixedPercentage()
    {
        // basket of items worth 200
        // expect 20% discount to be applied to the total
    }

    [Test, Ignore("support product-specific discounts")]
    public void ThenFruitOnlyDiscountAppliesOnlyToFruits()
    {
        // basket with fruit + non-fruit → only fruit discounted
    }

    [Test, Ignore("implement tiered discount logic")]
    public void ThenTieredDiscountGivesHigherDiscountsForLargerTotals()
    {
        // total under 100 → 0%
        // total 100–499 → 5%
        // total 500+ → 10%
    }

    [Test, Ignore("combine multiple discounts")]
    public void ThenMultipleDiscountsCanBeCombined()
    {
        // combine two strategies
        // e.g. tiered + fruit

        // *nudge nudge* composition *wink wink*
    }
    
    /*
     * pls don't edit the tests below this line i'll cry :(
     */

    [Test]
    public void ThenStrategiesGiveNonNegativeDiscounts()
    {
        foreach (var strategy in GetDiscountStrategies())
        {
            var items = new[]
            {
                new BasketItem("banana", 100m),
                new BasketItem("steak", 400m)
            };

            var calc = new DiscountCalculator(strategy);
            var total = calc.ApplyDiscount(items);

            Assert.That(total, Is.GreaterThanOrEqualTo(0m), 
                $"Strategy {strategy.GetType().Name} returned a negative total after discount");
            Assert.That(total, Is.LessThanOrEqualTo(items.Sum(i => i.Price)),
                $"Strategy {strategy.GetType().Name} returned a total higher than the original basket total");
        }
    }

    [Test]
    public void ThenEmptyBasketsReturnsZeroDiscounts()
    {
        foreach (var strategy in GetDiscountStrategies())
        {
            var calc = new DiscountCalculator(strategy);
            var items = Array.Empty<BasketItem>();

            var total = calc.ApplyDiscount(items);

            Assert.That(total, Is.EqualTo(0m),
                $"Strategy {strategy.GetType().Name} returned a non-zero total for empty basket");
        }
    }

    [Test]
    public void ThenZeroPriceItemsDoNotAffectDiscounts()
    {
        foreach (var strategy in GetDiscountStrategies())
        {
            var withFreebie = new[]
            {
                new BasketItem("freebie", 0m),
                new BasketItem("paid", 200m)
            };
            
            var withoutFreebie = new[]
            {
                new BasketItem("paid", 200m)
            };

            var calc = new DiscountCalculator(strategy);
            var withFreebieTotal = calc.ApplyDiscount(withFreebie);
            var withoutFreebieTotal = calc.ApplyDiscount(withoutFreebie);

            Assert.That(withFreebieTotal, Is.EqualTo(withoutFreebieTotal),
                $"Strategy {strategy.GetType().Name} handled zero-price items inconsistently");
        }
    }

    [Test]
    public void ThenNegativePricesAreHandledSafely()
    {
        foreach (var strategy in GetDiscountStrategies())
        {
            var items = new[] { new BasketItem("curse", -100m) };
            var calc = new DiscountCalculator(strategy);

            Assert.Throws<Exception>(() => calc.ApplyDiscount(items),
                $"Strategy {strategy.GetType().Name} did not throw exception for negative price");
        }
    }
    
    [Test]
    public void ThenDiscountsAreConsistent()
    {
        foreach (var strategy in GetDiscountStrategies())
        {
            var items = new[] {
                new BasketItem("banana", 100m),
                new BasketItem("steak", 400m)
            };

            var calc = new DiscountCalculator(strategy);
            var first = calc.ApplyDiscount(items);
            for (int i = 0; i < 5; i++)
            {
                var next = calc.ApplyDiscount(items);
                Assert.That(next, Is.EqualTo(first), 
                    $"Inconsistent discount from {strategy.GetType().Name} on iteration {i}");
            }
        }
    }
    
}
