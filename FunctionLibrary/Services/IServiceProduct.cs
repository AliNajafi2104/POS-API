using FunctionLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLibrary.Services
{
    public interface IServiceProduct
    {
        Task<List<Product>> GetProducts();

        Task<Product> GetProduct(string barcode);

        Task DeleteProduct(string barcode);

        Task UpdateProductAsync(string barcode, Product updatedProduct);

        Task CreateProductAsync(Product product);
    }
}
