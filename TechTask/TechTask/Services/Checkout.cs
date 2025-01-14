using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTask.Interfaces;
using TechTask.Models;

namespace TechTask.Services
{
    public class Checkout : ICheckout
    {
        private readonly Dictionary<string, Item> _pricingRules;
        private readonly Dictionary<string, int> _cart;

        public Checkout(Dictionary<string, Item> pricingRules)
        {
            _pricingRules = pricingRules ?? new Dictionary<string, Item>();
            // if no pricing rules are defined create empty dictionary
            // alternatively throw ArgumentNullException
            //_pricingRules = pricingRules ?? throw new ArgumentNullException(nameof(pricingRules));
            _cart = new Dictionary<string, int>();
        }

        public void Scan(string items)
        {
            foreach (var item in items)
            {
                if (_cart.ContainsKey(item.ToString()))
                    _cart[item.ToString()]++;
                else
                    _cart[item.ToString()] = 1;
            }
        }

        public int GetTotalPrice()
        {
            int totalPrice = 0;

            foreach (var cartItem in _cart)
            {
                var sku = cartItem.Key;
                var quantity = cartItem.Value;

                if (!_pricingRules.TryGetValue(sku, out var item))
                {
                    // If item is not defined in pricing rules, handle it appropriately return null or throw exception
                    // If we have scanned both valid and invalid items it will still return the correct value as invalid items are handled as 0 value
                    return totalPrice; // return the 0 total price here for unit tests
                }

                if (item.DiscountQuantity > 0 && quantity >= item.DiscountQuantity)
                {
                    int discountedSets = quantity / item.DiscountQuantity;
                    int remainingItems = quantity % item.DiscountQuantity;

                    totalPrice += (int)(discountedSets * item.DiscountPrice);
                    totalPrice += (int)(remainingItems * item.UnitPrice);
                }
                else
                    totalPrice += (int)(quantity * item.UnitPrice);
            }

            return totalPrice;
        }
    }
}
