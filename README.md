# open/closed principle kata (c#)

## overview

this kata’s all about solidifying your grip on the **open/closed principle**, the idea that software entities 
 should be **open for extension but closed for modification**.

you’ll build and test discount strategies for a shopping basket, with the goal of adding new discounts 
**without changing existing code**.

## what you get

- a core `DiscountCalculator` class that can apply any discount strategy implementing `IDiscountStrategy`
- a discount strategy example (`RegularCustomerDiscount`)
- a basket model (`BasketItem`) representing an item with a price
- some initial tests that dynamically tests base behavior of discounts 

## kata goals

- **practice** designing with O/C in mind: new discounts mean *adding* classes, *not changing* existing ones
- **write tests** that verify discounts work correctly and consistently across multiple implementations
- **explore** edge cases like empty baskets, zero or negative prices are handled correctly
- **extend** with your own discount strategies (tiered discounts, product-specific discounts, combined discounts, etc.)

## setup

1. clone repo
2. open solution in your favorite IDE (rider or visual studio i guess if you want to)
3. restore nuget packages (nunit, etc.)
4. run tests (all tests use nunit)

## your mission

- implement missing discount strategies (see ignored tests for suggestions)
  - some will be failing - it's your job to fix them 👀
- add your own creative discount strategies that adhere to `IDiscountStrategy`
- keep core calculator and existing strategies unchanged
- write new tests or improve existing ones for your strategies

## hints & tips

- use interfaces & polymorphism to stay open for extension
- keep constructor parameters simple or empty for easy reflection-based test discovery
- leverage test case sources to run tests across multiple strategies dynamically
- edge cases matter: what should happen with empty baskets, zero-priced items, or invalid inputs?
- consider how to combine multiple discounts without breaking O/C

## bonus challenge
- build a composite discount strategy that combines multiple discounts safely while still adhering 
to `IDiscountStrategy`. 

---

happy coding — keep it open, keep it closed, or something like that. 
