using Domain.Models.Contract;
using Domain.Models.Customer;
using Domain.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductContractConfiguration : IEntityTypeConfiguration<ProductContract>
{
    public void Configure(EntityTypeBuilder<ProductContract> builder)
    {
        builder.Property(c => c.SoftwareVersion)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.AdditionalSupportYears)
            .IsRequired();

        builder.Property(c => c.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.Status)
            .IsRequired();

        builder.Property(c => c.SigningDate);

        builder.Ignore(c => c.TotalUpdateYears);
        builder.Ignore(c => c.IsSigned);
        builder.Ignore(c => c.IsActive);
        builder.Ignore(c => c.TotalPaid);
        builder.Ignore(c => c.IsFullyPaid);

        builder.OwnsOne(c => c.PaymentWindow, paymentWindow =>
        {
            paymentWindow.Property(pw => pw.StartDate)
                .HasColumnName("PaymentWindowStartDate")
                .IsRequired();

            paymentWindow.Property(pw => pw.EndDate)
                .HasColumnName("PaymentWindowEndDate")
                .IsRequired();
        });

        builder.HasOne(c => c.Customer)
            .WithMany(c => c.Contracts)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.SoftwareProduct)
            .WithMany()
            .HasForeignKey(c => c.SoftwareProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Payments)
            .WithOne(p => p.PurchaseContract)
            .HasForeignKey(p => p.SoftwareContractId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
