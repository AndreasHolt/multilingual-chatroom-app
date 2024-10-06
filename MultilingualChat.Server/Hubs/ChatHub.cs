using Microsoft.AspNetCore.SignalR;
using MultilingualChat.Server.Services;

namespace MultilingualChat.Server.Hubs;

public class ChatHub : Hub
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
        // TODO: Add caching of messages that have already been translated, for users with the same language
        // TODO: Maybe let users join late, and then be presented previous messages
        var sender = await _userManager.GetUserAsync(Context.ConnectionId);
        if (sender == null) return;

        var connectedUsers = await GetAllUsers();
        var connectedUsersList = connectedUsers.ToList();
        var translationTasks = new Dictionary<string, Task<string>>();

        foreach (var connectedUser in connectedUsersList.Where(u => u.Language != sender.Language))
        {
            // Populate the translation tasks
            if (!translationTasks.ContainsKey(connectedUser.Language))
            {
                translationTasks[connectedUser.Language] =
                    _translationService.TranslateAsync(message, sender.Language, connectedUser.Language);
            }
        }

        await Task.WhenAll(translationTasks.Values);

        var sendTasks = connectedUsersList.Select(
            async connectedUser => // Create a sequence of tasks, one for each user
            {
                var translatedMessage = connectedUser.Language == sender.Language
                    ? message
                    : await translationTasks[connectedUser.Language];

                await Clients.Client(connectedUser.ConnectionId)
                    .SendAsync("SendMessage", translatedMessage, sender.Username);
                Console.WriteLine("Message sent to " + connectedUser.ConnectionId);
            });

        await Task.WhenAll(sendTasks);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var users = await _userManager.GetAllUsersAsync();

        return users;
    }
}