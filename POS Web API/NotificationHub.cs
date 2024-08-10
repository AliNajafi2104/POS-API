using Microsoft.AspNetCore.SignalR;
using POS_API.Models;
public class NotificationHub : Hub
{
    // Method that clients can call to send data
    public async Task SendProduct(ProductResponse result)
    {
        // Broadcast the product to all connected clients
        await Clients.All.SendAsync("ReceiveProduct", result);
    }
}
