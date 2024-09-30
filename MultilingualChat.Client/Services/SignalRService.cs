using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace MultilingualChat.Client.Services;


// The purpose of this service is to help manage a SignalR connection, s.t. it can be used throughout the application
// This service will be registered as a singleton in App.axaml.cs

public class SignalRService
{
    private HubConnection _connection;

    public SignalRService()
    {
        _connection = new HubConnectionBuilder().WithUrl("http://localhost:5041/chat").Build();
    }

    public async Task StartConnectionAsync(string username, string language)
    {
        try
        {
            await _connection.StartAsync();
            // TODO: Redesign to establish connection at "Confirm" button click, rather than at startup
            await _connection.SendAsync("JoinChat", username, language);
            Console.WriteLine("Connected to SignalR hub, connection id is " + _connection.ConnectionId);
            Console.WriteLine("SignalR Hub: Username and language are " + username + " " + language);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed connecting to SignalR hub: {ex.Message}");
        }
    }

    public async Task StopConnectionAsync()
    {
        Console.WriteLine("STOPPING!");
        await _connection.StopAsync();
    }

    public HubConnection GetConnection()
    {
        return _connection;
    }


}