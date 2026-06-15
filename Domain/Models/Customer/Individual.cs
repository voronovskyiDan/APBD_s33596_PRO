using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Customer
{
    public class IndividualCustomer : Customer
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Pesel { get; private set; }

        public IndividualCustomer(
            string firstName,
            string lastName,
            string pesel,
            string address,
            string email,
            string phoneNumber)
            : base(address, email, phoneNumber, CustomerType.Individual)
        {
            FirstName = firstName;
            LastName = lastName;
            Pesel = pesel;
        }

        public void UpdateName(string firstName, string lastName)
        {
            if (IsDeleted)
                throw new ConflictException("Cannot update deleted customer.");

            FirstName = firstName;
            LastName = lastName;
        }

        public override void Delete()
        {
            if (Type == CustomerType.Company)
                throw new ConflictException("Company customers cannot be deleted.");

            IsDeleted = true;
        }
    }
}
