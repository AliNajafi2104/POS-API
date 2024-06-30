using Mysqlx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FunctionLibrary.Models;
using searchengine123.Models;
namespace searchengine123
{
    public class TransactionService
    {
        private readonly HttpClient _httpClient;


        public TransactionService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7267/"); // Replace with your API base URL


        }
            public async Task<TransactionService> registerTransaction(TransactionDTO transaction)
        {
            try
            {
                string json = JsonConvert.SerializeObject(transaction);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    TransactionService createdTransaction = JsonConvert.DeserializeObject<TransactionService>(responseData);
                    return createdTransaction;
                }
                else
                {
                    throw new Exception($"Error saving transaction: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving transaction: {ex.Message}", ex);
            }




        }



        public async Task<List<Transaction>> GetTransactions()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Transaction"); // Adjust the endpoint URL as per your API

            if (response.IsSuccessStatusCode)
            {
                // Read the response content and deserialize into a List<Transaction> object
                string responseData = await response.Content.ReadAsStringAsync();
                List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(responseData);
                return transactions;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {

                
                return null; // Or handle the case where no transactions are found
            }
            else
            {
                // Handle other error cases
                string errorMessage = $"Error fetching transactions. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}";
                throw new HttpRequestException(errorMessage);
            }
        }

    }



}

