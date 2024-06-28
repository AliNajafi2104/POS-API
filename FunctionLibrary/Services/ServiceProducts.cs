using FunctionLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace FunctionLibrary.Services
{
    public class ServiceProducts: IServiceProduct
    {
        private readonly DataContext _context;

        public ServiceProducts(DataContext context)
        {
            _context = context;
        }

       public Task<List<Product>> GetProducts()
        {
            var products =  _context.Products.ToListAsync();
            return products;
        }



        async public Task<Product> GetProduct(string barcode)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(e => e.Barcode == barcode);

            return entity;
        }

        public async Task DeleteProduct(string barcode)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Barcode == barcode);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task UpdateProductAsync(string barcode, Product updatedProduct)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Barcode == barcode);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with barcode {barcode} not found.");
            }
            _context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);
            existingProduct.Barcode = barcode;
            await _context.SaveChangesAsync();
        }


        public async Task CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

    }
}
