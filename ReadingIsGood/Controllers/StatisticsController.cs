using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadingIsGood.Business.Services.Interface;
using ReadingIsGood.Core.Models;

namespace ReadingIsGood.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public StatisticsController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<Response>> Get()
        {
            return Ok(await _orderService.GetMonthlyStatistics());
        }
    }
}
