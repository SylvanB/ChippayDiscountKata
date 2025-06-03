using System.Reflection;
using ChippayDiscounts.Discounts;
using ChippayDiscounts.Core;

namespace ChippayDiscounts.Tests;

public class DiscountTests : DiscountTestsBase
{
    [TestCaseSource(nameof(StrategyCases))]
    public void ThenStrategiesGiveNonNegativeDiscounts(IDiscountStrategy strategy)
    {
        var items = new[]
        {
            new BasketItem("banana", 100m),
            new BasketItem("steak", 400m)
        };

        var calc = new DiscountCalculator(strategy);
        var total = calc.ApplyDiscount(items);

        Assert.That(total, Is.GreaterThanOrEqualTo(0m));
        Assert.That(total, Is.LessThanOrEqualTo(items.Sum(i => i.Price)));
    }

    [TestCaseSource(nameof(StrategyCases))]
    public void ThenEmptyBasketsReturnsZeroDiscounts(IDiscountStrategy strategy)
    {
        var calc = new DiscountCalculator(strategy);
        var items = Array.Empty<BasketItem>();

        var total = calc.ApplyDiscount(items);

        Assert.That(total, Is.EqualTo(0m));
    }

    [TestCaseSource(nameof(StrategyCases))]
    public void ThenZeroPriceItemsDoNotAffectDiscounts(IDiscountStrategy strategy)
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

        Assert.That(withFreebieTotal, Is.EqualTo(withoutFreebieTotal)); // 5% of 200, adjust if needed per strategy
    }

    [TestCaseSource(nameof(StrategyCases))]
    public void ThenNegativePricesAreHandledSafely(IDiscountStrategy strategy)
    {
        var items = new[] { new BasketItem("curse", -100m) };

        var calc = new DiscountCalculator(strategy);

        Assert.Throws<Exception>(() => calc.ApplyDiscount(items));
    }
    
    [TestCaseSource(nameof(StrategyCases))]
    public void ThenDiscountsAreConsistent(IDiscountStrategy strategy)
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
        // expect 20% discount = 160
    }
    
    [Test, Ignore("support product-specific discounts")]
    public void ThenFruitOnlyDiscountAppliesOnlyToFruits()
    {
        // fruit + non-fruit → only fruit discounted
    }

    [Test, Ignore("combine multiple discounts")]
    public void ThenMultipleDiscountsCanBeCombined()
    {
        // combine two strategies
        // e.g. tiered + fruit
    }

    [Test, Ignore("implement tiered discount logic")]
    public void ThenTieredDiscountGivesHigherDiscountsForLargerTotals()
    {
        // under 100 → 0%
        // 100–499 → 5%
        // 500+ → 10%
    }

}
