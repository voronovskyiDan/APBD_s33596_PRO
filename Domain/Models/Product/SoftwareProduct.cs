using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Discount;

namespace Domain.Models.Product
{
    public class SoftwareProduct
    {
        public int Id { get; private set; }

        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public string Version { get; private set; } = null!;
        public SoftwareCategory Category { get; private set; }
        public decimal AnnualLicensePrice { get; private set; }

        public List<Discount.Discount> Discounts { get; set; } = [];

        private SoftwareProduct() { }

        public SoftwareProduct(
            int id,
            string name,
            string description,
            string currentVersion,
            SoftwareCategory category,
            decimal annualLicensePrice)
        {
            Id = id;
            Name = name;
            Description = description;
            Version = currentVersion;
            Category = category;
            AnnualLicensePrice = annualLicensePrice;
        }

        public void UpdateDetails(string name, string description, string version, SoftwareCategory category)
        {
            Name = name;
            Description = description;
            Version = version;
            Category = category;
        }

        public Discount.Discount? GetHighestActiveDiscount(DiscountType type)
        {
            return Discounts
                .Where(d => d.Type == type && d.IsActive(DateTime.UtcNow))
                .OrderByDescending(d => d.Percentage)
                .FirstOrDefault();
        }
    }
}
