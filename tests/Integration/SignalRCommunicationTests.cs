using Microsoft.AspNetCore.SignalR.Client;

namespace tests.Integration;
using Xunit;
using MultilingualChat.Server.Hubs;
using MultilingualChat.Client.Services;

public class SignalRCommunicationTests
{
    [Fact]
    public async Task TestClientCommunicationSignalR()
    {
        var client1 = new SignalRService();
        var client2 = new SignalRService();
        
        await client1.StartConnectionAsync("Client 1", "en");
        await client2.StartConnectionAsync("Client 2", "en");

        var testMessage = "This is a test message";
        var senderName = "Client 1";
        
        HubConnection client1Connection = client1.GetConnection();
        HubConnection client2Connection = client1.GetConnection();
        
        await client1Connection.InvokeAsync("SendMessage", testMessage, senderName);
        
        client2Connection.On<string, string>("SendMessage", (message, sender) => {
            Assert.Equal(testMessage, message);
            Assert.Equal(senderName, sender);
        });
        
        
        
        



    }
}