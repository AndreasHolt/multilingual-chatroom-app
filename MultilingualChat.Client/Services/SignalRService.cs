using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace MultilingualChat.Client.Services;


// The purpose of this service is to help manage a SignalR connection, s.t. it can be used throughout the application
// This service will be registered as a singleton in App.axaml.cs

public class SignalRService
{
    private HubConnection _connection;
    private string _roomId;

    public SignalRService()
    {
        _connection = new HubConnectionBuilder().WithUrl("http://localhost:5041/chat").Build();
    }

    public async Task StartConnectionAsync(string username, string language, string roomId, string chatColor)
    {
        try
        {
            await _connection.StartAsync();
            await _connection.SendAsync("JoinChat", username, language, roomId, chatColor);
            _roomId = roomId;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed connecting to SignalR hub: {ex.Message}");
            Console.WriteLine("Stopping connection");
            await StopConnectionAsync();
        }
    }

    public async Task StopConnectionAsync()
    {
        Console.WriteLine("Stopping connection!");
        await _connection.StopAsync();
    }

    public HubConnection GetConnection()
    {
        return _connection;
    }

    public string GetRoomId()
    {
        return _roomId;
    }

}