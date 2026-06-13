using Application.DTOs.Customer;
using Application.DTOs.Customer.Add;
using Application.DTOs.Customer.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
        public Task<CustomerDto> CreateCompany(AddComapnyDto addCustomerDto);
        public Task<CustomerDto> CreateIndividual(AddIndividualDto addIndividualDto);
        public Task<CustomerDto?> GetById(int id);
        public Task Delete(int id);
        public Task UpdateCompany(int id, UpdateCompanyDto updateCompanyDto);
        public Task UpdateIndividual(int id, UpdateIndividualDto updateIndividualDto);
    }
}
