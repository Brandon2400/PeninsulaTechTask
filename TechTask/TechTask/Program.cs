using System.Collections.Generic;
using System;
using TechTask.Interfaces;
using TechTask.Models;
using TechTask.Services;

namespace TechTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var pricingRules = new Dictionary<string, Item>
        {
            { "A", new Item("A", 50, 3, 130) },
            { "B", new Item("B", 30, 2, 45) },
            { "C", new Item("C", 20) },
            { "D", new Item("D", 15) }
        };

            ICheckout checkout = new Checkout(pricingRules);

            try
            {
                //checkout.Scan("ABAABC");
                checkout.Scan("X");

                Console.WriteLine($"Total Price: {checkout.GetTotalPrice()}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error - {e.Message}");
            }
        }
    }
}