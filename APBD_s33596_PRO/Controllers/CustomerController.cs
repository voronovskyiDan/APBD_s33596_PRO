using Application.DTOs.Customer;
using Application.DTOs.Customer.Add;
using Application.DTOs.Customer.Update;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_s33596_PRO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _customerService.GetById(id);
            if(res == null)
                return NotFound("No customer with such id");
            return Ok(res);
        }

        [HttpPost("/company/create")]
        public async Task<IActionResult> CreateCompany(AddComapnyDto addComapnyDto) {
            var customer = await _customerService.CreateCompany(addComapnyDto);
            
            return CreatedAtAction(nameof(GetById), new { id = customer.Id} , customer);
        }

        [HttpPost("/company/update/{id:int}")]
        public async Task<IActionResult> UpdateCompany(int id, UpdateCompanyDto updateCompany)
        {
            await _customerService.UpdateCompany(id, updateCompany);
            return NoContent();
        }


        [HttpPost("/individual/create")]
        public async Task<IActionResult> CreateIndividual(AddIndividualDto addIndividualDto)
        {
            var customer = await _customerService.CreateIndividual(addIndividualDto);
            
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }
        [HttpPost("/individual/update/{id:int}")]
        public async Task<IActionResult> UpdateIndividual(int id, UpdateIndividualDto updateIndividualDto)
        {
            await _customerService.UpdateIndividual(id, updateIndividualDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.Delete(id);
            return NoContent();
        }


    }
}
