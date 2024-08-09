
using POS_API.DTO;



public interface IServiceProduct
{
    Task<List<Product>> GetProducts();

    Task<Product> GetProduct(string barcode);

    Task DeleteProduct(string barcode);

    Task UpdateProductAsync(string barcode, Product updatedProduct);

    Task CreateProductAsync(Product product);


    Task PostBasket(string barcode);

    Task<List<ProductBasketDTO>> GetBasket();

    Task ResetBasket();
  
}

