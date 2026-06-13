using Application.DTOs.Contract;
using Application.DTOs.Contract.Accept;
using Application.DTOs.Contract.Add;
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
    public interface IContractService
    {
        public Task<ContractDto> CreateContract(AddContractDto addContractDto);
        public Task AcceptPayment(int id, AcceptPaymentDto acceptPayment);
        public Task<ContractDto?> GetById(int id);
    }
}
