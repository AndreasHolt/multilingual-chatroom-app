using Microsoft.AspNetCore.SignalR;

namespace MultilingualChat.Server.Hubs;

public class ChatHub: Hub
{
    // SendMessage can be called by a connected client to send a message to all other clients.
    // 1. My guess is that the client connects to endpoint (/chat).
    // 2. They can then invoke the exposed function SendMessage at that endpoint, which then sends the message to all other endpoints
    public async Task SendMessage(string message, string user)
    {
        Console.WriteLine("INVOKED");
        await Clients.All.SendAsync("ReceiveMessage", message, user);
        
    }
    
    
}