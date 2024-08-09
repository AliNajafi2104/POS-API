using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using POS_API.DTO;
using POS_API.Models;
using System.Net.Http;





public class ServiceProducts : IServiceProduct
{
    private readonly HttpClient _httpClient;
    private readonly DataContext _context;

    public ServiceProducts(HttpClient httpClient, DataContext context)
    {
        _httpClient = httpClient;
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



    public async Task getdata()
    {
        string baseUrl = "https://poswebapi20240714125856.azurewebsites.net";
        string endpoint = "/api/product";

        try
        {
            // Step 1: Get the list of products from the remote API
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(baseUrl + endpoint);

            if (products != null)
            {
                // Step 2: Create each product in the local database
                foreach (var product in products)
                {

                    if(product.Barcode.Length<255)
                    {

                    Product productDTOMove = new Product
                    {
                        Name = product.Name,
                        Price = product.Price,
                        Barcode = product.Barcode
                    };
                    await CreateProductAsync(productDTOMove);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            throw new Exception("An error occurred while retrieving or saving products.", ex);
        }
    }



}

