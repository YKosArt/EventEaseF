using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EventEaseF.Models;

public enum UserRole
{
    User,
    Administrator
}

public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "Ім'я обов'язкове")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Ім'я має бути від 2 до 100 символів")]
    public string Name { get; set; } = string.Empty;


    public UserRole Role { get; set; } = UserRole.User;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public List<int> RegisteredEventIds { get; set; } = new();
    
}
