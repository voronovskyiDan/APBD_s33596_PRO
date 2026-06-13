using Domain.Models.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    internal class PriceCalculator
    {
        private const decimal ReturningCustomerDiscountPercent = 5m;

        public static decimal ApplyDiscounts(decimal basePrice, Discount? discount, bool isReturningCustomer)
        {
            var price = basePrice;

            if (discount != null)
                price -= price * (discount.Percentage / 100m);

            if (isReturningCustomer)
                price -= price * (ReturningCustomerDiscountPercent / 100m);

            return Math.Round(price, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal ApplyReturningCustomerDiscountOnly(decimal basePrice)
        {
            return ApplyDiscounts(basePrice, discount: null, isReturningCustomer: true);
        }
    }
}
