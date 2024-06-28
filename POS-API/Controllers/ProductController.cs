using FunctionLibrary.Models;
using FunctionLibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var products = await _serviceProduct.GetProducts();
            return Ok(products);
        }

        [HttpGet("{barcode}")]
        public async Task<IActionResult> GetProduct(string barcode)
        {
            var product = await _serviceProduct.GetProduct(barcode);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            await _serviceProduct.CreateProductAsync(product);
            return Ok(); // Return an IActionResult to satisfy the return type
        }


        [HttpDelete("{barcode}")]
        public async Task<IActionResult> DeleteProduct(string barcode)
        {
            await _serviceProduct.DeleteProduct(barcode);
            return Ok();
        }

        [HttpPut("{barcode}")]
        public async Task<IActionResult> UpdateProduct(string barcode, Product product)
        {
            await _serviceProduct.UpdateProductAsync(barcode, product);
            return Ok();
        }
    }

}
