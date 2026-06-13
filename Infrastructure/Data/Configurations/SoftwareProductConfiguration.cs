using Domain.Models.Discount;
using Domain.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class SoftwareProductConfiguration : IEntityTypeConfiguration<SoftwareProduct>
{
    public void Configure(EntityTypeBuilder<SoftwareProduct> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.Version)
            .HasMaxLength(50);

        builder.Property(p => p.AnnualLicensePrice)
            .HasPrecision(18, 2);

        builder.HasMany(p => p.Discounts)
            .WithMany(d => d.Products);
    }
}
