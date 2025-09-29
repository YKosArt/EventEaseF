using EventEaseF.Data;
using EventEaseF.Models;
using Microsoft.EntityFrameworkCore;

namespace EventEaseF.Services;

public class EventDbService
{
    private readonly ApplicationDbContext _db;

    public EventDbService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<Event>> GetAllEventsAsync() =>
        await _db.Events.Where(e => e.IsActive).OrderBy(e => e.Date).ToListAsync();

    public async Task<Event?> GetEventByIdAsync(int id) =>
        await _db.Events.FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

    public async Task<bool> CreateEventAsync(Event newEvent)
    {
        newEvent.CreatedAt = DateTime.Now;
        _db.Events.Add(newEvent);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateEventAsync(Event updatedEvent)
    {
        var existing = await _db.Events.FindAsync(updatedEvent.Id);
        if (existing == null) return false;

        _db.Entry(existing).CurrentValues.SetValues(updatedEvent);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var existing = await _db.Events.FindAsync(id);
        if (existing == null) return false;

        existing.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Dictionary<string, int>> GetEventStatisticsAsync()
    {
        var events = await _db.Events.Where(e => e.IsActive).ToListAsync();

        return new Dictionary<string, int>
        {
            ["TotalEvents"] = events.Count,
            ["UpcomingEvents"] = events.Count(e => e.Date > DateTime.Now),
            ["PastEvents"] = events.Count(e => e.Date <= DateTime.Now),
            ["TotalParticipants"] = events.Sum(e => e.CurrentParticipants),
            ["AverageParticipants"] = events.Any()
                ? (int)events.Average(e => e.CurrentParticipants)
                : 0
        };
    }

public async Task<List<Event>> SearchEventsAsync(string searchTerm)
{
    if (string.IsNullOrWhiteSpace(searchTerm))
        return await GetAllEventsAsync();

    var term = searchTerm.Trim().ToLowerInvariant();

    return await _db.Events
        .Where(e => e.IsActive &&
            (EF.Functions.Like(e.Name.ToLower(), $"%{term}%") ||
             EF.Functions.Like(e.Description.ToLower(), $"%{term}%") ||
             EF.Functions.Like(e.Location.ToLower(), $"%{term}%")))
        .OrderBy(e => e.Date)
        .ToListAsync();
}


}
