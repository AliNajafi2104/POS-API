using Microsoft.AspNetCore.SignalR;

public class NotificationHub : Hub
{
    // Method that clients can call to send data
    public async Task SendBarcode(string barcode)
    {
        // Broadcast the barcode to all connected clients
        await Clients.All.SendAsync("ReceiveBarcode", barcode);
    }
}
