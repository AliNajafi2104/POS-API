
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {


        private readonly IServiceTransaction _service;

        public TransactionController(IServiceTransaction service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _service.GetTransactions();
            return Ok(transactions);
        }




        [HttpPost]
        public async Task<IActionResult> PostTransaction(transactionDTO transaction)
        {
            await _service.RegisterTransaction(transaction);
            return Ok();
        }



        [HttpPost("/transactionDetail")]
        public async Task<IActionResult> PostTransactionDetail(List<Product> products)
        {
            await _service.RegisterTransactionDetails(products);
            return Ok();
        }




    }
}
