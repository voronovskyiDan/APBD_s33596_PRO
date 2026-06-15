using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Contract
{
    public class ContractPayment
    {
        public int Id { get; private set; }
        public int CustomerId { get; private set; }
        public Customer.Customer Customer { get; private set; } = null!;
        public int SoftwareContractId { get; private set; }
        public ProductContract PurchaseContract { get; private set; } = null!;
        public decimal AmountPln { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public bool IsRefunded { get; private set; }
        private ContractPayment() { }
        public ContractPayment(Customer.Customer customer, ProductContract contract, decimal amountPln)
        {
            if (amountPln <= 0)
                throw new BadRequestException("Payment amount must be positive.");
            Customer = customer;
            PurchaseContract = contract;
            AmountPln = amountPln;
            PaymentDate = DateTime.UtcNow;
            IsRefunded = false;
        }
        internal void MarkRefunded() => IsRefunded = true;

    }
}
