using Application.DTOs.Common;
using Application.DTOs.Contract.Add;
using Application.DTOs.Subscription.Add;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APBD_s33596_PRO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _subscriptionService.GetById(id);
            if (res == null)
                return NotFound("No subscription with such id");
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(AddSubscriptionDto addSubscription)
        {
            var contract = await _subscriptionService.CreateSubscription(addSubscription);
            return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
        }

        [HttpPost("{id:int}/accept-payment")]
        public async Task<IActionResult> AcceptPayment(int id, AcceptPaymentDto acceptPaymentDto)
        {
            await _subscriptionService.AcceptPayment(id, acceptPaymentDto);
            return NoContent();
        }
    }
}
