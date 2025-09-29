using System.ComponentModel.DataAnnotations;

namespace EventEaseF.Models;

public class Event
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Назва обов'язкова")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Назва має бути від 3 до 200 символів")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Опис обов'язковий")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Опис має бути від 10 до 1000 символів")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Дата обов'язкова")]
    public DateTime Date { get; set; } = DateTime.Now.AddDays(1);

    [Required(ErrorMessage = "Місце проведення обов'язкове")]
    [StringLength(300, MinimumLength = 5, ErrorMessage = "Місце має бути від 5 до 300 символів")]
    public string Location { get; set; } = string.Empty;

    [Range(1, 10000, ErrorMessage = "Кількість учасників має бути від 1 до 10000")]
    public int MaxParticipants { get; set; } = 10;
    public int CurrentParticipants { get; set; } = 0;


    public List<string> RegisteredUserIds { get; set; } = new();

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string ImageUrl { get; set; } = "https://via.placeholder.com/400x200?text=Event";

    [Required(ErrorMessage = "Категорія є обов'язковою")]
    public string? Category { get; set; } = "";

    [Range(0, 10000, ErrorMessage = "Ціна має бути від 0 до 10000")]
    public decimal Price { get; set; }

    public List<Registration> Registrations { get; set; } = new();

    // Обчислювані властивості
    public int AvailableSpots => MaxParticipants - RegisteredUserIds.Count;
    public bool IsFull => RegisteredUserIds.Count >= MaxParticipants;
    public bool IsUpcoming => Date > DateTime.Now;
    public bool IsPast => Date <= DateTime.Now;





}


