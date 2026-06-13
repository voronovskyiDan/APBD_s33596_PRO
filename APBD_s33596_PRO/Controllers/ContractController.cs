using Application.DTOs.Contract.Accept;
using Application.DTOs.Contract.Add;
using Application.Interfaces;
using Application.Services;
using Domain.Models.Customer;
using Microsoft.AspNetCore.Mvc;

namespace APBD_s33596_PRO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _contractService.GetById(id);
            if (res == null)
                return NotFound("No contract with such id");
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract(AddContractDto addContractDto)
        {
            var contract = await _contractService.CreateContract(addContractDto);
            return CreatedAtAction(nameof(GetById), new { }, contract);
        }

        [HttpPost("{id:int}/accept-payment")]
        public async Task<IActionResult> AcceptPayment(int id, AcceptPaymentDto acceptPaymentDto)
        {
             await _contractService.AcceptPayment(id, acceptPaymentDto);
            return NoContent();
        }
    }
}
