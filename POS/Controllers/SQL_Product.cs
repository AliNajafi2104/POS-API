using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;

using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Math.EC.Multiplier;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using FunctionLibrary.Models;

namespace searchengine123
{
    public class SQL_Product
    {

        private readonly HttpClient _httpClient;


        public SQL_Product()
        {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://vvmapi.azurewebsites.net/api/Product/"); // Replace with your API base URL


        }

        public async Task<Product> GetProductFromApiAsync(string barcode)
        {
            
            HttpResponseMessage response = await _httpClient.GetAsync(barcode);

            if (response.IsSuccessStatusCode)
            {
               
                string responseData = await response.Content.ReadAsStringAsync();
                Product product = JsonConvert.DeserializeObject<Product>(responseData);
                return product;
            }

            if(response.StatusCode==HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception("Error\n" +response );
            }
        }


        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                string json = JsonConvert.SerializeObject(product);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("", content);

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







    }
}