using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadingIsGood.Business.Services.Interface;
using ReadingIsGood.Core.Models;
using ReadingIsGood.Core.Models.Requests;

namespace ReadingIsGood.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("Insert")]
        [Produces("application/json")]
        public async Task<ActionResult<Response>> Insert([FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                var apiResponse = new Response(false);

                foreach (var errorMessage in ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }))
                {
                    apiResponse.Errors.Add(new Error()
                    {
                        ErrorMessage = string.Concat(errorMessage.Key, "->", errorMessage.Errors.FirstOrDefault().ErrorMessage)
                    });

                }
                return BadRequest(apiResponse);
            }

            try
            {
                return Ok(await _customerService.Insert(request));
            }
            catch (Exception ex)
            {
                var response = new Response(false);
                response.Errors.Add(new Error { ErrorMessage = ex.Message });
                return StatusCode(500, response);
            }
        }
    }
}
    