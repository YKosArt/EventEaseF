using EventEaseF.Models;
using EventEaseF.Data;
using Microsoft.EntityFrameworkCore;

namespace EventEaseF.Services;

public class RegistrationService(EventDbService eventService, UserService userService, ApplicationDbContext db)
{
    private readonly EventDbService _eventService = eventService;
    private readonly UserService _userService = userService;
    private readonly ApplicationDbContext _db = db;

    public async Task<bool> RegisterUserAsync(Registration registration)
    {
        var eventItem = await _eventService.GetEventByIdAsync(registration.EventId);
        var user = await _userService.GetUserByIdAsync(registration.UserId);

        if (eventItem == null || user == null || eventItem.CurrentParticipants >= eventItem.MaxParticipants)
            return false;

        var alreadyRegistered = await _db.Registrations
            .AnyAsync(r => r.EventId == registration.EventId && r.UserId == registration.UserId);

        if (alreadyRegistered)
            return false;

        _db.Registrations.Add(registration);

        eventItem.CurrentParticipants++;
        eventItem.RegisteredUserIds ??= new();
        if (!eventItem.RegisteredUserIds.Contains(registration.UserId))
            eventItem.RegisteredUserIds.Add(registration.UserId);

        user.RegisteredEventIds ??= new();
        if (!user.RegisteredEventIds.Contains(registration.EventId))
            user.RegisteredEventIds.Add(registration.EventId);

        _db.Events.Update(eventItem);
        _db.Users.Update(user);

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Registration>> GetRegistrationsForEventAsync(int eventId) =>
        await _db.Registrations
            .Where(r => r.EventId == eventId)
            .OrderBy(r => r.RegisteredAt)
            .ToListAsync();

    public async Task<bool> UnregisterFromEventAsync(string userId, int eventId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        var eventItem = await _eventService.GetEventByIdAsync(eventId);

        if (user == null || eventItem == null)
            return false;

        user.RegisteredEventIds?.Remove(eventId);
        eventItem.RegisteredUserIds?.Remove(userId);
        eventItem.CurrentParticipants = Math.Max(0, eventItem.CurrentParticipants - 1);

        _db.Events.Update(eventItem);
        _db.Users.Update(user);

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Event>> GetUserEventsAsync(string userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user?.RegisteredEventIds == null)
            return [];

        var events = await _eventService.GetAllEventsAsync();
        return events
            .Where(e => user.RegisteredEventIds.Contains(e.Id))
            .OrderBy(e => e.Date)
            .ToList();
    }

    public async Task<List<Event>> GetUserUpcomingEventsAsync(string userId)
    {
        var events = await GetUserEventsAsync(userId);
        return events.Where(e => e.IsUpcoming).ToList();
    }

    public async Task<List<Event>> GetUserPastEventsAsync(string userId)
    {
        var events = await GetUserEventsAsync(userId);
        return events.Where(e => e.IsPast).ToList();
    }

    public async Task<bool> IsUserRegisteredAsync(string userId, int eventId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        return user?.RegisteredEventIds?.Contains(eventId) ?? false;
    }

    public async Task<int> GetEventRegistrationCountAsync(int eventId)
    {
        var eventItem = await _eventService.GetEventByIdAsync(eventId);
        return eventItem?.RegisteredUserIds?.Count ?? 0;
    }
}


