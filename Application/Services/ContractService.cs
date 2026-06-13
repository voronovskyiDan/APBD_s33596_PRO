using Application.DTOs.Contract;
using Application.DTOs.Contract.Accept;
using Application.DTOs.Contract.Add;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.Models.Contract;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly ICustomerRespository _customerRespository;
        private readonly IProductRepository _productRepository;
        public ContractService(
            IContractRepository contractRepository,
            ICustomerRespository customerRespository,
            IProductRepository productRepository)
        {
            _contractRepository = contractRepository;
            _customerRespository = customerRespository;
            _productRepository = productRepository;
        }

        public async Task AcceptPayment(int id, AcceptPaymentDto acceptPayment)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null)
                throw new NotFoundException("No contract with such id");
            contract.
        }

        public async Task<ContractDto> CreateContract(AddContractDto addContractDto)
        {
            var customer = await _customerRespository.GetByIdAsync(addContractDto.CustomerId);
            if (customer == null)
                throw new NotFoundException("No customer with such id");

            var product = await _productRepository.GetByIdWithDiscountsAsync(addContractDto.ProdcutId);
            if (product == null)
                throw new NotFoundException("No product with such id");

            var contract = ProductContract.Create(
                            customer,
                            product,
                            new PaymentWindow(
                                addContractDto.StartDate,
                                addContractDto.EndDate),
                            addContractDto.AdditionalSupportYears
                            );

            await _contractRepository.SaveChangesAsync();

            return new ContractDto
            {
                Id = contract.Id,
                CustomerId = contract.CustomerId,
                ProfuctId = contract.SoftwareProductId,
                SoftwareVersion = contract.SoftwareVersion,
                TotalPrice = contract.TotalPrice,
                SigningDate = contract.SigningDate,
                Status = contract.Status.ToString(),
            };
        }

        public async Task<ContractDto?> GetById(int id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null)
                return null;
            return new ContractDto {
                Id = contract.Id,
                CustomerId = contract.CustomerId,
                ProfuctId = contract.SoftwareProductId,
                SoftwareVersion = contract.SoftwareVersion,
                TotalPrice = contract.TotalPrice,
                SigningDate = contract.SigningDate,
                Status = contract.Status.ToString(),
            };
        }
    }
}
