using Microsoft.AspNetCore.SignalR;
public class NotificationHub : Hub
{
    // Method that clients can call to send data
    public async Task SendProduct(Product product)
    {
        // Broadcast the product to all connected clients
        await Clients.All.SendAsync("ReceiveProduct", product);
    }
}
