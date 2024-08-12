using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinformsGUI.Models;

namespace WinformsGUI
{
    public partial class Forside
    {
        private HubConnection _hubConnection;
        private async void InitializeSignalR()
        {
            // Initialize the connection to the SignalR hub
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://{Config.IP_ADDRESS}:2030/notificationHub") // Use the correct URL for your SignalR hub
                .Build();

            // Define how to handle incoming messages
            _hubConnection.On<ProductResponse>("ReceiveProduct", result =>
            {


                // Update the UI on the main thread
                Invoke(new Action(() =>
                {
                    if (result.Product != null)
                    {
                        scannedProducts.Add(result.Product);
                        UpdateDataGridView();
                    }
                    else
                    {
                        ProductNotFoundPopUp();
                        tbBarcodeCreate.Text = result.Barcode;
                    }


                }));
            });

            try
            {
                // Start the connection
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR connection started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
            }
        }
    }
}
