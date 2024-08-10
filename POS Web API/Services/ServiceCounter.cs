
using POS_API.Services.Interfaces;
using System.Data.Entity;

namespace POS_API.Services
{
    public class ServiceCounter : IServiceCounter
    {

        private readonly DataContext _context;

        public ServiceCounter(DataContext context)
        {
            _context = context;
        }


        public async Task AddProductCount(Product product)
        {
            try
            {
                var productToUpdate = _context.Product.FirstOrDefault(p => p.Barcode == product.Barcode);

                if (productToUpdate != null)
                {
                    if (product.Name != null)
                    {
                        productToUpdate.Name = product.Name;
                    }

                    // Since Price is non-nullable, directly assign it
                    if (product.Price >= 0) // Assuming that a valid Price is non-negative
                    {
                        productToUpdate.Price = product.Price;
                    }

                    productToUpdate.Count = product.Count;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException($"Product with Barcode '{product.Barcode}' not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred adding product count", ex);
            }
        }





        public async Task ResetProductCounters()
        {
            var products = await _context.Product.ToListAsync();
            foreach (var product in products)
            {
                product.Count = 0;
            }
            await _context.SaveChangesAsync();
        }


        public async Task<decimal> GetTotalPriceOfCounters()
        {
            decimal TotalSum = 0;
            foreach (var product in _context.Product)
            {
                TotalSum += product.Price * (product.Count ?? 0);
            }
            return TotalSum;
        }

    }
}
