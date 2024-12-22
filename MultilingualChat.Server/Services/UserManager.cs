using System.Collections.Concurrent;

namespace MultilingualChat.Server.Services;

/// <summary>
///     Manages connected users
/// </summary>
public class User(string username, string language, string connectionId, string roomId, string color)
{
    public string Username { get; set; } = username;
    public string Language { get; set; } = language;
    public string ConnectionId { get; set; } = connectionId;
    public string RoomId { get; set; } = roomId;
    public string Color { get; set; } = color;
}

public class UserManager : IUserManager
{
    private readonly ConcurrentDictionary<string, User> _users = new();

    public Task AddUserAsync(string connectionId, string username, string language, string roomId, string color)
    {
        _users[connectionId] = new User(username, language, connectionId, roomId, color);
        return Task.CompletedTask;
    }

    // Gets all users in a room
    public Task<IEnumerable<User>> GetAllUsersInRoomAsync(string roomId)
    {
        var usersInRoom = _users.Values.Where(u => u.RoomId == roomId).ToList();
        Console.WriteLine("Users in room " + roomId);
        Console.WriteLine("With usernames " + usersInRoom.Select(u => u.Username));
        foreach (var user in usersInRoom)
        {
            Console.WriteLine(
                $"Username: {user.Username}, Language: {user.Language}, ConnectionId: {user.ConnectionId}");
        }

        return Task.FromResult(usersInRoom.AsEnumerable());
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = _users.Values.ToList();
        Console.WriteLine("Users in user manager");
        Console.WriteLine(users.Count);
        foreach (var user in users)
        {
            Console.WriteLine(
                $"Username: {user.Username}, Language: {user.Language}, ConnectionId: {user.ConnectionId}");
        }

        return Task.FromResult(users.AsEnumerable());
    }

    public Task RemoveUserAsync(string connectionId)
    {
        _users.TryRemove(connectionId, out _);
        return Task.CompletedTask;
    }

    public Task<User?> GetUserAsync(string connectionId)
    {
        _users.TryGetValue(connectionId, out var user);
        return Task.FromResult(user);
    }
}

public interface IUserManager
{
    Task AddUserAsync(string connectionId, string username, string language, string roomId, string color);

    Task<IEnumerable<User>> GetAllUsersAsync();
    
    Task<IEnumerable<User>> GetAllUsersInRoomAsync(string roomId);

    Task<User?> GetUserAsync(string connectionId);
    
}