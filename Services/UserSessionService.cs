using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Threading.Tasks;

public class UserSessionService
{
    private readonly ProtectedSessionStorage _storage;
    private const string Key = "UserSession";

    public string? UserId { get; private set; }
    public string? UserName { get; private set; }
    public string? Email { get; private set; }
    public string? Role { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(UserId);

    public event Action? OnSessionChanged;

    public UserSessionService(ProtectedSessionStorage storage)
    {
        _storage = storage;
    }

    public async Task SetSessionAsync(string userId, string userName, string email, string role)
    {
        UserId = userId;
        UserName = userName;
        Email = email;
        Role = role;

        await _storage.SetAsync(Key, new SessionData
        {
            UserId = userId,
            UserName = userName,
            Email = email,
            Role = role
        });

        OnSessionChanged?.Invoke(); // 🔔 реактивне оновлення
    }

    public async Task LoadSessionAsync()
    {
        var result = await _storage.GetAsync<SessionData>(Key);
        if (result.Success && result.Value is not null)
        {
            UserId = result.Value.UserId;
            UserName = result.Value.UserName;
            Email = result.Value.Email;
            Role = result.Value.Role;

            OnSessionChanged?.Invoke(); // 🔔 реактивне оновлення
        }
    }

    public async Task ClearSessionAsync()
    {
        UserId = null;
        UserName = null;
        Email = null;
        Role = null;

        await _storage.DeleteAsync(Key);
        OnSessionChanged?.Invoke(); // 🔔 реактивне оновлення
    }

    private class SessionData
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
