using Domain.Models.Discount;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.Property(d => d.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(d => d.Percentage)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(d => d.Type)
            .IsRequired();

        builder.OwnsOne(d => d.Range, range =>
        {
            range.Property(r => r.Start)
                .HasColumnName("RangeStart")
                .IsRequired();

            range.Property(r => r.End)
                .HasColumnName("RangeEnd")
                .IsRequired();
        });
    }
}
