using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS_API.DTO;
using POS_API.Services;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly IServiceCounter _serviceCounter;

        public CounterController(IServiceCounter serviceCounter)
        {
            _serviceCounter = serviceCounter;
        }



        [HttpPatch("count")]
        public async Task<IActionResult> PostProductCount(ProductDTO product)
        {
            try
            {
                await _serviceCounter.AddProductCount(product);
                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Error occurred while adding product count" });
            }
        }


        [HttpPost("ResetCounter")]
        public async Task<IActionResult> ResetProductCounter()
        {
            try
            {
                await _serviceCounter.ResetProductCounters();
                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Error occurred while reseting product counters" });
            }
        }

        [HttpGet("TotalCounter")]
        public async Task<IActionResult> GetTotalCounterPrice()
        {
            var Total = await _serviceCounter.GetTotalPriceOfCounters();
            return Ok(Total);
        }
    }
}
