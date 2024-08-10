using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using POS_API.Models;



namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceProduct _serviceProduct;


        public ProductController(IServiceProduct serviceProduct, IHubContext<NotificationHub> hubContext)
        {
            _serviceProduct = serviceProduct;
            _hubContext = hubContext;
        }


        private readonly IHubContext<NotificationHub> _hubContext;


        [HttpPost("SignalR/{barcode}")]
        public async Task<IActionResult> PostBarcode(string barcode)
        {

            
            Product product = await _serviceProduct.GetProduct(barcode);
            
            
            ProductResponse productResponse = new ProductResponse
            {
                Product = product,
                Barcode = barcode
            };
            // Broadcast the product data to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveProduct", productResponse);
            return Ok();
        }


        [HttpGet("move")]
        public async Task getdata()
        {
            await _serviceProduct.getdata();

        }



        #region CRUD
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _serviceProduct.GetProducts();
                return Ok(products);
            }

            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message + "Innerexception: " + ex.InnerException });
            }
        }

        [HttpGet("{barcode}")]
        public async Task<IActionResult> GetProduct(string barcode)
        {
            try
            {

                var result = await _serviceProduct.GetProduct(barcode);
              
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message + "Innerexception: " + ex.InnerException });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _serviceProduct.CreateProductAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { barcode = product.Barcode }, product);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message + "Innerexception: " + ex.InnerException });
            }
        }


        [HttpDelete("{barcode}")]
        public async Task<IActionResult> DeleteProduct(string barcode)
        {
            try
            {
                await _serviceProduct.DeleteProduct(barcode);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message + "Innerexception: " + ex.InnerException });
            }
        }

        [HttpPut("{barcode}")]
        public async Task<IActionResult> UpdateProduct(string barcode, Product product)
        {
            try
            {
                await _serviceProduct.UpdateProductAsync(barcode, product);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message + "Innerexception: " + ex.InnerException });
            }
        }
        #endregion

    }

}

