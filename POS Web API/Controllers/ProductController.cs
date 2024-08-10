
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.SignalR;



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
            // Broadcast the barcode data to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveBarcode", barcode);
            return Ok();
        }


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

                return Ok(ex.Message + ex.InnerException);
            }
        }

        [HttpGet("{barcode}")]
        public async Task<IActionResult> GetProduct(string barcode)
        {
            try
            {

                var product = await _serviceProduct.GetProduct(barcode);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { message = "Error occurred while fetching product" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody]Product product)
        {
            if(!ModelState.IsValid)
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
               
                return StatusCode(500, new { message = "Error occurred while creating product" });
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
               
                return StatusCode(500, new { message = "Error occurred while deleting product" });
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
               
                return StatusCode(500, new { message = "Error occurred while updating product" });
            }
        }



        [HttpGet("move")]
        public async Task getdata()
        {
        await    _serviceProduct.getdata();
          
        }

    }

}

