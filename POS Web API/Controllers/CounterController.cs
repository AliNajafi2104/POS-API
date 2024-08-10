using Microsoft.AspNetCore.Mvc;
using POS_API.Services.Interfaces;

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

                return StatusCode(500, new { message = ex.Message + "Innerexception:" + ex.InnerException });
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

                return StatusCode(500, new { message = ex.Message + "Innerexception:" + ex.InnerException });
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

                return StatusCode(500, new { message = ex.Message + "Innerexception:" + ex.InnerException });
            }





        }




    }
}
