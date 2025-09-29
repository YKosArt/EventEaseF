using EventEaseF.Models;
using Microsoft.AspNetCore.Identity;

namespace EventEaseF.Services;

public static class IdentityInitializer
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Administrator", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"[Seed] Створено роль: {role}");
            }
        }
    }

    public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
    {
        var users = new[]
        {
            new ApplicationUser
            {
                UserName = "admin@eventease.com",
                Email = "admin@eventease.com",
                Name = "Адміністратор системи",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Role = UserRole.Administrator
            },
            new ApplicationUser
            {
                UserName = "user@example.com",
                Email = "user@example.com",
                Name = "Тестовий користувач",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Role = UserRole.User
            }
        };

        foreach (var user in users)
{
    if (string.IsNullOrWhiteSpace(user.Email))
    {
        Console.WriteLine("[Seed] Email не задано для одного з користувачів.");
        continue;
    }

    var existing = await userManager.FindByEmailAsync(user.Email);
    if (existing == null)
    {
        var result = await userManager.CreateAsync(user, GetDefaultPassword(user.Email));
        if (result.Succeeded)
        {
            var role = user.Role.ToString();
            await userManager.AddToRoleAsync(user, role);
            Console.WriteLine($"[Seed] Створено користувача: {user.Email} з роллю {role}");
        }
        else
        {
            Console.WriteLine($"[Seed] Помилка при створенні {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}

    }

    private static string GetDefaultPassword(string email)
    {
        return email switch
        {
            "admin@eventease.com" => "Admin@123",
            "user@example.com" => "User@123",
            _ => "Password@123"
        };
    }
}