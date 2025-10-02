using EventEaseF.Components;
using EventEaseF.Services;
using EventEaseF.Data;
using EventEaseF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Доступ до HttpContext
builder.Services.AddHttpContextAccessor();

// 🔧 База даних SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔧 Сервіс роботи з подіями
builder.Services.AddScoped<EventDbService>();

// 🔐 ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// 🧱 Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// 🛠️ Custom services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<AuthService>();

// 🔧 Логування
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddConsole();
builder.Services.AddRazorPages();

var app = builder.Build();

// 🔧 Ініціалізація бази та ролей
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated(); // 🔥 Створює таблиці, якщо їх немає

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    await IdentityInitializer.SeedRolesAsync(roleManager);
    await IdentityInitializer.SeedUsersAsync(userManager);
    await EventSeeder.SeedEventsAsync(db);
}

// 🔧 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// 🔗 Маршрутизація компонентів
// app.MapStaticAssets(); // ❌ недоступно в .NET 8 — залишено закоміченим
app.MapRazorPages();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();
