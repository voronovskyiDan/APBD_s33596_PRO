using Domain.Models.Contract;
using Domain.Models.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ContractPaymentConfiguration : IEntityTypeConfiguration<ContractPayment>
{
    public void Configure(EntityTypeBuilder<ContractPayment> builder)
    {
        builder.Property(p => p.AmountPln)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(p => p.IsRefunded)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(p => p.Customer)
            .WithMany(c => c.ContractPayments)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.PurchaseContract)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.SoftwareContractId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
