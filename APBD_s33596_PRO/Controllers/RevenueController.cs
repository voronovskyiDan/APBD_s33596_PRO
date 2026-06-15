using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APBD_s33596_PRO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueService _revenueService;
        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent([FromQuery] int? productId = null)
        {
            return Ok(await _revenueService.GetCurrentRevenueAsync(productId));
        }

        [HttpGet("predicted")]
        public async Task<IActionResult> GetPredicted([FromQuery] int? productId = null)
        {
            return Ok(await _revenueService.GetPredictedRevenueAsync(productId));
        }
    }
}
