using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Customer
{
    public class CompanyCustomer : Customer
    {
        public string CompanyName { get; private set; }

        public string KrsNumber { get; private set; }

        public CompanyCustomer(
            string companyName,
            string krsNumber,
            string address,
            string email,
            string phoneNumber)
            : base(address, email, phoneNumber, CustomerType.Company)
        {
            CompanyName = companyName;
            KrsNumber = krsNumber;
        }

        public void UpdateCompanyName(string companyName)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Cannot update deleted company.");

            CompanyName = companyName;
        }

        public override void Delete()
        {
            throw new InvalidOperationException("Company customers cannot be deleted.");
        }
    }
}
