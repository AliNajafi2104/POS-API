
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using POS_API.DTO;


namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceProduct _serviceProduct;
        private readonly ILogger<ProductController> _logger;


        public ProductController(IServiceProduct serviceProduct, ILogger<ProductController> logger)
        {
            _serviceProduct = serviceProduct;
            _logger = logger;
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
                _logger.LogError(ex, "Error gettings products ");
                return StatusCode(500, new { message = "Error occurred while fetching products" });
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
                _logger.LogError(ex, "Error getting product");
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
                _logger.LogError(ex, "Error");
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
                _logger.LogError(ex, "Error");
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
                _logger.LogError(ex, "Error");
                return StatusCode(500, new { message = "Error occurred while updating product" });
            }
        }

        [HttpPost("Scan/{barcode}")]
        public async Task<IActionResult> PostBasket(string barcode)
        {
            try
            {
                await _serviceProduct.PostBasket(barcode);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("Basket")]
        public async Task<IActionResult> GetBasket()
        {
            try
            {
                var products = await _serviceProduct.GetBasket();
                return Ok(products);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error gettings products ");
                return StatusCode(500, new { message = "Error occurred while fetching products" });
            }
        }

        [HttpPost("ResetBasket")]
        public async Task<IActionResult> ResetBasket()
        {
            try
            {
                await _serviceProduct.ResetBasket();
                return Ok(new { message = "Basket has been reset successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception (if you have a logger)
                // _logger.LogError(ex, "Error occurred while resetting the basket");
                return StatusCode(500, new { message = "Error occurred while resetting the basket" });
            }
        }
    }

}


