using Application.DTOs.Common;
using Application.DTOs.Contract;
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

        public async Task AcceptPayment(int contractId, AcceptPaymentDto acceptPayment)
        {
            var contract = await _contractRepository.GetByIdWithPaymentsAsync(contractId);
            if (contract == null)
                throw new NotFoundException("No contract with such id");

            var customer = await _customerRespository.GetByIdAsync(acceptPayment.CustomerId);
            if (customer == null)
                throw new NotFoundException("No customer with such id");

            ContractPayment contractPayment = new ContractPayment(customer, contract, acceptPayment.Amount);

            try {
                contract.RegisterPayment(contractPayment);
            }
            finally { 
                await _contractRepository.SaveChangesAsync();
            }
        }

        public async Task<ContractDto> CreateContract(AddContractDto addContractDto)
        {
            var customer = await _customerRespository.GetByIdAsync(addContractDto.CustomerId!.Value);
            if (customer == null)
                throw new NotFoundException("No customer with such id");

            var product = await _productRepository.GetByIdWithDiscountsAsync(addContractDto.ProdcutId!.Value);
            if (product == null)
                throw new NotFoundException("No product with such id");

            var contract = ProductContract.Create(
                            customer,
                            product,
                            new PaymentWindow(
                                addContractDto.StartDate!.Value,
                                addContractDto.EndDate!.Value),
                            addContractDto.AdditionalSupportYears
                            );

            int id = await _contractRepository.AddAsync(contract);

            return new ContractDto
            {
                Id = id,
                CustomerId = contract.CustomerId,
                ProductId = contract.SoftwareProductId,
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
                ProductId = contract.SoftwareProductId,
                SoftwareVersion = contract.SoftwareVersion,
                TotalPrice = contract.TotalPrice,
                SigningDate = contract.SigningDate,
                Status = contract.Status.ToString(),
            };
        }
    }
}
