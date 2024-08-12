using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WinformsGUI.Models;

namespace WinformsGUI
{
    public class ProductService
    {

        private readonly HttpClient _httpClient;


        public ProductService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"http://{Config.IP_ADDRESS}:2030/");


        }


        public async Task<Product> GetProductFromApiAsync(string barcode)
        {

            string requestUri = $"api/product/{barcode}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            string responseData = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Product product = JsonConvert.DeserializeObject<Product>(responseData);
                return product;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception("Error\n" + response);
            }
        }



        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                string json = JsonConvert.SerializeObject(product);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("api/product", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Product createdProduct = JsonConvert.DeserializeObject<Product>(responseData);
                    return createdProduct;
                }
                else
                {
                    throw new Exception($"Error creating product: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating product: {ex.Message}", ex);
            }
        }


        public async Task DeleteProduct(string barcode)
        {
            try
            {
                string url = $"api/product/{barcode}";

                HttpResponseMessage response = await _httpClient.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
                else
                {
                    throw new Exception($"Error deleting product: {response.StatusCode} - {response.ReasonPhrase}");

                }
            }

            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting product:{ex.Message}", ex);
            }
        }








    }


}
