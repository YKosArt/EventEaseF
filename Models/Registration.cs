using System.ComponentModel.DataAnnotations;

namespace EventEaseF.Models;

public class Registration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    public DateTime RegisteredAt { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;
    public bool AgreeToTerms { get; set; }
    public bool SubscribeToNewsletter { get; set; }

    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Company { get; set; }
    public string? DietaryRequirements { get; set; }
    public string SpecialRequests { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}
