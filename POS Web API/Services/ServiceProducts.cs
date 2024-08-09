using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using POS_API.DTO;





public class ServiceProducts : IServiceProduct
{
    private readonly DataContext _context;

    public ServiceProducts(DataContext context)
    {
        _context = context;
    }




    public Task<List<Product>> GetProducts()
    {
        try
        {

            var products = _context.Product.ToListAsync();
            return products;
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while fetching products ", ex);
        }
    }

    async public Task<Product> GetProduct(string barcode)
    {
        try
        {

            var entity = await _context.Product.FirstOrDefaultAsync(e => e.Barcode == barcode);
            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception("Error occured fetching product ", ex);

        }

    }

    public async Task DeleteProduct(string barcode)
    {
        try
        {

            var product = await _context.Product.FirstOrDefaultAsync(p => p.Barcode == barcode);
            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error occured deleting product ", ex);
        }
    }


    public async Task UpdateProductAsync(string barcode, Product updatedProduct)
    {
        try
        {

            var existingProduct = await _context.Product.FirstOrDefaultAsync(p => p.Barcode == barcode);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with barcode {barcode} not found.");
            }
            _context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);
            existingProduct.Barcode = barcode;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error occured updating product ", ex);
        }


    }


    public async Task CreateProductAsync(Product product)
    {
        try

        {

            _context.Product.Add(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error occured creating product ", ex);
        }
    }

    public async Task PostBasket(string barcode)
    {

        try
        {
            var product = await GetProduct(barcode);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            var productBasket = new ProductBasketDTO
            {
                Name = product.Name,
                Barcode = product.Barcode,
                Price = product.Price
            };

            _context.ProductBasketDTO.Add(productBasket); // Add entity to DbSet
            await _context.SaveChangesAsync(); // Save changes to the database
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while posting to basket", ex);
        }

    }

    public async Task<List<ProductBasketDTO>> GetBasket()
    {
        try
        {
            var products = await _context.ProductBasketDTO.ToListAsync();
            return products;
        }
        catch
        {
            return null;
        }
    }


    public async Task ResetBasket()
    {
        try
        {
            // Assuming ProductBasketDTO is a DbSet in your DbContext
            var items = _context.ProductBasketDTO.ToList();

            // Remove all items from the basket
            _context.ProductBasketDTO.RemoveRange(items);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception if you have a logging mechanism
            
            // Optionally, rethrow the exception or handle it as needed
            throw new Exception("Error occurred while resetting the basket", ex);
        }
    }

}

