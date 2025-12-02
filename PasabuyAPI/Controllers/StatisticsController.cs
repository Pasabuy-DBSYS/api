using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Services.Interfaces;
using System.Security.Claims;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController(IStatisticsService statisticsService) : ControllerBase
    {
        private readonly IStatisticsService _statisticsService = statisticsService;

        [Authorize(Policy = "CustomerOnly")]
        [HttpGet("customer")]
        public async Task<ActionResult<CustomerStatisticsResponseDTO>> GetCustomerStatisticsAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var customerId))
                return BadRequest("Invalid user ID format.");

            var statistics = await _statisticsService.GetCustomerStatistics(customerId);
            return Ok(statistics);
        }

        [Authorize(Policy = "CourierOnly")]
        [HttpGet("courier")]
        public async Task<ActionResult<CourierStatisticsResponseDTO>> GetCourierStatisticsAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var courierId))
                return BadRequest("Invalid user ID format.");

            var statistics = await _statisticsService.GetCourierStatistics(courierId);
            return Ok(statistics);
        }
    }
}
