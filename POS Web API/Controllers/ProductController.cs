
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

        public ProductController(IServiceProduct serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _serviceProduct.GetProducts();
                return Ok(products);
            }

            catch
            {
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
            catch
            {
                return StatusCode(500, new { message = "Error occurred while fetching product" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            try
            {
                await _serviceProduct.CreateProductAsync(product);
                return Ok();
            }
            catch
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
                return Ok();
            }
            catch
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
                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Error occurred while updating product" });
            }
        }



    }

}
