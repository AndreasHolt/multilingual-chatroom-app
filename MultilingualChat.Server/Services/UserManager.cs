using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace MultilingualChat.Server.Services;

/// <summary>
///     Manages connected users
/// </summary>

public class User
{
    public User(string username, string language, string connectionId)
    {
        Username = username;
        Language = language;
        ConnectionId = connectionId;
    }
    public string Username { get; set; }
    public string Language { get; set; }
    public string ConnectionId { get; set; }
}


public class UserManager: IUserManager
{
    private readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();
    public Task AddUserAsync(string connectionId, string username, string language)
    {
        _users[connectionId] = new User(username, language, connectionId);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = _users.Values.ToList();
        Console.WriteLine("Users in user manager");
        Console.WriteLine(users.Count);
        foreach (var user in users)
        {
            Console.WriteLine($"Username: {user.Username}, Language: {user.Language}, ConnectionId: {user.ConnectionId}");
        }
        return Task.FromResult(users.AsEnumerable());
    }
    
    public Task RemoveUserAsync(string connectionId)
    {
        _users.TryRemove(connectionId, out _);
        return Task.CompletedTask;
    }
    
    
    
}

public interface IUserManager
{
    Task AddUserAsync(string connectionId,string username, string language);
    
    Task<IEnumerable<User>> GetAllUsersAsync();
}