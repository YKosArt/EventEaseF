using EventEaseF.Models;
using Microsoft.EntityFrameworkCore;

namespace EventEaseF.Data;

public static class EventSeeder
{
    public static async Task SeedEventsAsync(ApplicationDbContext db)
    {
        if (await db.Events.AnyAsync()) return;

        var events = new List<Event>
        {
            new Event
            {
                Name = ".NET 9: Новинки та тренди",
                Description = "Конференція про нові можливості .NET 9",
                Date = DateTime.Now.AddDays(15),
                Location = "Київ, Палац Спорту",
                MaxParticipants = 500,
                CurrentParticipants = 0,
                Category = "Tech",
                Price = 0,
                ImageUrl = "/images/dotnet9.jpg",
                
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Event
            {
                Name = "Blazor Web App Workshop",
                Description = "Практичний воркшоп з Blazor",
                Date = DateTime.Now.AddDays(7),
                Location = "Львів, Innovation Hub",
                MaxParticipants = 50,
                CurrentParticipants = 0,
                Category = "Workshop",
                Price = 0,
                ImageUrl = "/images/blazor.jpg",
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        };

        db.Events.AddRange(events);
        await db.SaveChangesAsync();
    }
}