using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MultilingualChat.Server.Services;

namespace MultilingualChat.Server.Hubs;

public class ChatHub: Hub
{
    private readonly IUserManager _userManager;
    private readonly ITranslationService _translationService;

    public ChatHub(IUserManager userManager, ITranslationService translationService)
    {
        _userManager = userManager;
        _translationService = translationService;
    }
    public async Task JoinChat(string username, string language)
    {
        Console.WriteLine("Joining chat JoinChat");
        var contextConnectionId = Context.ConnectionId;
        Console.WriteLine("Context connection id is " + contextConnectionId);
        await _userManager.AddUserAsync(contextConnectionId, username, language);
    }
    // SendMessage can be called by a connected client to send a message to all other clients.
    // 1. The client connects to endpoint (/chat).
    // 2. They can then invoke the exposed function SendMessage at that endpoint, which then sends the message to all other endpoints
    public async Task SendMessage(string message, string user)
    {
        var connectedUsers = await GetAllUsers();
        var sender = await _userManager.GetUserAsync(Context.ConnectionId);
        if (sender == null)
        {
            Console.WriteLine("Sender not found");
            return;
        }

        foreach (var connectedUser in connectedUsers)
        {
            Console.WriteLine("Sending message to " + connectedUser.ConnectionId);
            try
            {
                // Add translation middleware here
                // Maybe do caching of messages that have already been translated, for users with the same language?
                // Maybe let users join late, and then be presented previous messages?
                var translatedMessage = await _translationService.TranslateAsync(message, sender.Language, connectedUser.Language);
                // var translatedMessage = message;
                await Clients.Client(connectedUser.ConnectionId).SendAsync("SendMessage", translatedMessage, sender.Username);
                Console.WriteLine("Message sent to " + connectedUser.ConnectionId);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send message to {connectedUser.ConnectionId}: {ex.Message}");
            }
        }
        // await Clients.All.SendAsync("SendMessage", message, user);
        
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var users = await _userManager.GetAllUsersAsync();
        
        return users;
    }
    
    
    
}
