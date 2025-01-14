using TechTask.Models;
using TechTask.Services;

namespace TechTaskTests
{
    public class CheckOutTests
    {
        [Fact]
        public void SingleItemScanShouldReturnCorrectPrice()
        {
            // original pricing rules
            var pricingRules = new Dictionary<string, Item>
            {
                { "A", new Item("A", 50, 3, 130) },
                { "B", new Item("B", 30, 2, 45) },
                { "C", new Item("C", 20) },
                { "D", new Item("D", 15) }
            };

            var checkout = new Checkout(pricingRules);

            checkout.Scan("A");
            var total = checkout.GetTotalPrice();

            Assert.Equal(50, total);  // Expecting 50
        }

        [Fact]
        public void NoItemsScanned_TotalPriceShouldBeZero()
        {
            var pricingRules = new Dictionary<string, Item>
            {
                { "A", new Item("A", 50) },
                { "B", new Item("B", 30) }
            };

            var checkout = new Checkout(pricingRules);

            var total = checkout.GetTotalPrice();

            Assert.Equal(0, total);  // No items scanned, total should be 0.
        }

        [Fact]
        public void SingleItemScanInvalidItemShouldReturnZeroTotal()
        {
            // original pricing rules
            var pricingRules = new Dictionary<string, Item>
            {
                { "A", new Item("A", 50, 3, 130) },
                { "B", new Item("B", 30, 2, 45) },
                { "C", new Item("C", 20) },
                { "D", new Item("D", 15) }
            };

            var checkout = new Checkout(pricingRules);

            checkout.Scan("X");
            var total = checkout.GetTotalPrice();

            Assert.Equal(0, total);  // Expecting 0 as X isnt defined in the pricing rules
        }

        [Fact]
        public void MultipleItemScansShouldApplyCorrectDiscounts()
        {
            // altered pricing rules
            var pricingRules = new Dictionary<string, Item>
            {
                { "A", new Item("A", 50, 2, 85) },
                { "B", new Item("B", 30, 2, 45) },
                { "C", new Item("C", 20) },
                { "D", new Item("D", 15) }
            };

            var checkout = new Checkout(pricingRules);

            checkout.Scan("AABBAC");
            // 2 A's, should apply discount 
            // 2 B's, should apply discount
            // 1 A, no discount
            // 1 C, no discount

            var total = checkout.GetTotalPrice();

            Assert.Equal(200, total);  // Expecting 200
        }

        [Fact]
        public void DiscountAppliesCorrectlyToMultipleScans()
        {
            // original pricing rules
            var pricingRules = new Dictionary<string, Item>
            {
                { "A", new Item("A", 50, 3, 130) },
                { "B", new Item("B", 30, 2, 45) },
                { "C", new Item("C", 20) },
                { "D", new Item("D", 15) }
            };

            var checkout = new Checkout(pricingRules);

            checkout.Scan("AAABBCD");
            // 3 A’s, should apply discount
            // 2 B’s, should apply discount
            // 1 C
            // 1 D

            var total = checkout.GetTotalPrice();

            Assert.Equal(210, total);  // Expecting 210 with applied discounts
        }

        [Fact]
        public void NoPricingRulesShouldHandleGracefully()
        {
            var checkout = new Checkout(null);  // No pricing rules provided

            var total = checkout.GetTotalPrice();

            Assert.Equal(0, total);  // No pricing rules, so total should be 0
        }

        [Fact]
        public void ScanBothValidAndInvalidItemsReturnsCorrectTotal()
        {
            // original pricing rules
            var pricingRules = new Dictionary<string, Item>
            {
                { "A", new Item("A", 50, 3, 130) },
                { "B", new Item("B", 30, 2, 45) },
                { "C", new Item("C", 20) },
                { "D", new Item("D", 15) }
            };

            var checkout = new Checkout(pricingRules);

            checkout.Scan("AAABBCDXYZ");

            var total = checkout.GetTotalPrice();

            Assert.Equal(210, total);  // Expecting 210 as X, Y and Z are set to return 0 in this instance
        }
    }
}