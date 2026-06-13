using Application.DTOs.Customer;
using Application.DTOs.Customer.Add;
using Application.DTOs.Customer.Update;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.Models.Customer;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRespository _customerRespository;

        public CustomerService(ICustomerRespository customerRespository)
        {
            _customerRespository = customerRespository;
        }

        public async Task<CustomerDto> CreateCompany(AddComapnyDto addCustomerDto)
        {
            CompanyCustomer customer = new CompanyCustomer(
                addCustomerDto.CompanyName,
                addCustomerDto.KrsNumber,
                addCustomerDto.Address,
                addCustomerDto.Email,
                addCustomerDto.PhoneNumber
                );
            
            int id = await _customerRespository.AddAsync(customer);

            return new CustomerDto
            {
                Id = id,
                Asddress = addCustomerDto.Address,
                Email = addCustomerDto.Email,
                PhoneNumber = addCustomerDto.PhoneNumber
            };
        }

        public async Task<CustomerDto> CreateIndividual(AddIndividualDto addIndividualDto)
        {
            IndividualCustomer customer = new IndividualCustomer(
               addIndividualDto.FirstName,
               addIndividualDto.LastName,
               addIndividualDto.Pesel,
               addIndividualDto.Address,
               addIndividualDto.Email,
               addIndividualDto.PhoneNumber
               );

            int id = await _customerRespository.AddAsync(customer);

            return new CustomerDto
            {
                Id = id,
                Asddress = addIndividualDto.Address,
                Email = addIndividualDto.Email,
                PhoneNumber = addIndividualDto.PhoneNumber
            };
        }

        public async Task Delete(int id)
        {
            var customer = await _customerRespository.GetByIdAsync(id);
            if (customer == null)
                throw new NotFoundException("User with such id doesnt exists");
            
            customer.Delete();
            
            await _customerRespository.SaveChangesAsync();
        }

        public async Task<CustomerDto?> GetById(int id)
        {
            var customer = await _customerRespository.GetByIdAsync(id);
            if (customer == null)
                return null;
            return new CustomerDto
            {
                Id = id,
                Asddress = customer.Address,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };
        }

        public async Task UpdateCompany(int id, UpdateCompanyDto updateCompanyDto)
        {
            var customer = await _customerRespository.GetByIdAsync(id);

            if (customer == null)
                throw new NotFoundException("User with such id doesn't exist");

            if (customer is not CompanyCustomer company)
                throw new BadRequestException("Customer is not a company");

            company.UpdateContact(
                updateCompanyDto.Address,
                updateCompanyDto.Email,
                updateCompanyDto.PhoneNumber);

            company.UpdateCompanyName(updateCompanyDto.CompanyName);

            await _customerRespository.SaveChangesAsync();
        }

        public async Task UpdateIndividual(int id, UpdateIndividualDto updateIndividualDto)
        {
            var customer = await _customerRespository.GetByIdAsync(id);

            if (customer == null)
                throw new NotFoundException("User with such id doesn't exist");

            if (customer is not IndividualCustomer individual)
                throw new BadRequestException("Customer is not a individual");

            individual.UpdateContact(
                updateIndividualDto.Address,
                updateIndividualDto.Email,
                updateIndividualDto.PhoneNumber);

            individual.UpdateName(updateIndividualDto.FirstName, updateIndividualDto.LastName);

            await _customerRespository.SaveChangesAsync();
        }
    }
}
