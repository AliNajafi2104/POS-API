//using Microsoft.AspNetCore.SignalR.Client;
//using System;
//using System.Media;
//using WinformsGUI.Models;

//namespace WinformsGUI
//{
//    public partial class Forside
//    {
//        private HubConnection _hubConnection;
//        private async void InitializeSignalR()
//        {

//            _hubConnection = new HubConnectionBuilder()
//                .WithUrl($"http://{Config.IP_ADDRESS}:2030/notificationHub")
//                .Build();


//            _hubConnection.On<ProductResponse>("ReceiveProduct", result =>
//            {



//                Invoke(new Action(() =>
//                {
//                    if (result.Product != null)
//                    {
//                        SystemSounds.Beep.Play();
//                        scannedProducts.Add(result.Product);
//                        UpdateDataGridView();
//                    }
//                    else
//                    {
//                        SystemSounds.Hand.Play();
//                        ProductNotFoundPopUp();
//                        tbBarcodeCreate.Text = result.Barcode;
//                    }


//                }));
//            });

//            try
//            {

//                await _hubConnection.StartAsync();
//                Console.WriteLine("SignalR connection started.");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
//            }
//        }
//    }
//}
