
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {


        private readonly IServiceTransaction _service;
        private readonly Logger<TransactionController> _logger;
        public TransactionController(IServiceTransaction service, Logger<TransactionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _service.GetTransactions();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return StatusCode(500, new { message = "An error occurred while fetching transactions." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostTransaction(transactionDTO transaction)
        {
            try
            {
                await _service.RegisterTransaction(transaction);
                return Ok();
            }
            catch (Exception ex)

            {
                _logger.LogError(ex, "Error");
                return StatusCode(500, new { message = "An error occurred while registering transaction." });
            }
        }

        [HttpPost("/transactionDetail")]
        public async Task<IActionResult> PostTransactionDetail(List<Product> products)
        {
            try
            {
                await _service.RegisterTransactionDetails(products);
                return Ok();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error");
                return StatusCode(500, new { message = "An error occurred while registering transaction details." });
            }
        }

    }
}
