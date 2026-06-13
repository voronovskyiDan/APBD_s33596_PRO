using Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Discount
{
    public class Discount
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public decimal Percentage { get; private set; }
        public DateRange Range { get; private set; } = null!;
        public DiscountType Type { get; private set; }

        public List<SoftwareProduct> Products { get; set; } = [];


        private Discount() { }

        public Discount(string name, decimal percentage, DateRange range, DiscountType type)
        {
            if (percentage <= 0 || percentage > 100)
                throw new ArgumentException("Invalid discount percentage.");

            Name = name;
            Percentage = percentage;
            Range = range;
            Type = type;
        }

        public bool IsActive(DateTime dt) => Range.Contains(dt);

    }
}
