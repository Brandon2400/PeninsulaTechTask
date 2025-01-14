using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTask.Models
{
    public class Item
    {
        public string SKU { get; set; }
        public decimal UnitPrice { get; set; }
        public int DiscountQuantity { get; set; }
        public decimal DiscountPrice { get; set; }

        public Item(string sku, decimal unitPrice, int discountQuantity = 0, decimal discountPrice = 0)
        {
            SKU = sku;
            UnitPrice = unitPrice;
            DiscountQuantity = discountQuantity;
            DiscountPrice = discountPrice;
        }
    }
}
