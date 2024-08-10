using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using POS_API.Services;
using System.Data.SqlClient;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly IServiceCounter _serviceCounter;
        private readonly ILogger<CounterController> _logger;

        public CounterController(IServiceCounter serviceCounter, ILogger<CounterController> logger)
        {
            _serviceCounter = serviceCounter;
            _logger = logger;
        }



        [HttpPatch("count")]
        public async Task<IActionResult> PostProductCount(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _serviceCounter.AddProductCount(product);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding product count");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reseting product Counters");
                return StatusCode(500, new { message = "Error occurred while reseting product counters" });
            }
        }

        [HttpGet("TotalCounter")]
        public async Task<IActionResult> GetTotalCounterPrice()
        {
            try
            {
                var Total = await _serviceCounter.GetTotalPriceOfCounters();
                return Ok(Total);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total count price");
                return StatusCode(500, new { message = "Error occurred while fetching total price of counters" });
            }





        }


      

    }
}
