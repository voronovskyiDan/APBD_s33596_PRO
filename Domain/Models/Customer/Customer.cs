using Domain.Exceptions;
using Domain.Models.Contract;
using Domain.Models.Product;
using Domain.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Customer
{
    public abstract class Customer
    {
        public int Id { get; private set; }

        public string Address { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string PhoneNumber { get; private set; } = null!;

        public CustomerType Type { get; private set; }

        public bool IsDeleted { get; protected set; }

        public List<ProductContract> Contracts { get; private set; } = [];
        public List<ProductSubscription> Subscriptions { get; private set; } = [];

        public List<ContractPayment> ContractPayments { get; private set; } = [];
        public ICollection<SubscriptionPayment> SubscriptionPayments { get; private set; } = [];

        protected Customer() { }

        protected Customer(
            string address,
            string email,
            string phoneNumber,
            CustomerType type)
        {
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            Type = type;
            IsDeleted = false;
        }

        public void UpdateContact(string address, string email, string phoneNumber)
        {
            if (IsDeleted)
                throw new ConflictException("Cannot update deleted customer.");

            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public bool IsReturningCustomer()
        {
            var hasSignedContract = Contracts.Any(c => c.IsSigned);
            var hasSubscription = Subscriptions.Any(s => s.Status == SubscriptionStatus.Active
                || s.Payments.Any());

            return hasSignedContract || hasSubscription;
        }

        public abstract void Delete();

        public void EnsureCanAcquireProduct(SoftwareProduct product)
        {
            var hasActiveContract = Contracts.Any(c =>
                c.SoftwareProductId == product.Id &&
                c.IsActive);

            if (hasActiveContract)
                throw new ConflictException(
                    "Customer already has an active contract for this product.");

            var hasActiveSubscription = Subscriptions.Any(s =>
                s.SoftwareProductId == product.Id &&
                s.Status == SubscriptionStatus.Active);

            if (hasActiveSubscription)
                throw new ConflictException(
                    "Customer already has an active subscription for this product.");
        }

    }
}
