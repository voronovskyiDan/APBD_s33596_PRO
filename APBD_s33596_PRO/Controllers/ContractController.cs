using Application.DTOs.Common;
using Application.DTOs.Contract.Add;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_s33596_PRO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
        }

        [HttpPost("{id:int}/accept-payment")]
        public async Task<IActionResult> AcceptPayment(int id, AcceptPaymentDto acceptPaymentDto)
        {
            await _contractService.AcceptPayment(id, acceptPaymentDto);
            return NoContent();
        }
    }
}
