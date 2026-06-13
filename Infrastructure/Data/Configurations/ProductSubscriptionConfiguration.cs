using Domain.Models.Customer;
using Domain.Models.Product;
using Domain.Models.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductSubscriptionConfiguration : IEntityTypeConfiguration<ProductSubscription>
{
    public void Configure(EntityTypeBuilder<ProductSubscription> builder)
    {
        builder.Property(s => s.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.RenewalPeriodMonths)
            .IsRequired();

        builder.Property(s => s.PricePerPeriod)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(s => s.Status)
            .IsRequired();

        builder.OwnsOne(s => s.SubscriptionWindow, window =>
        {
            window.Property(w => w.Start)
                .HasColumnName("SubscriptionWindowStart")
                .IsRequired();

            window.Property(w => w.End)
                .HasColumnName("SubscriptionWindowEnd")
                .IsRequired();
        });

        builder.HasOne(s => s.Customer)
            .WithMany(c => c.Subscriptions)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.SoftwareProduct)
            .WithMany()
            .HasForeignKey(s => s.SoftwareProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Payments)
            .WithOne(p => p.Subscription)
            .HasForeignKey(p => p.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
