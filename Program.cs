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


// 🔐 ASP.NET Core Identity — обов’язково для UserManager, SignInManager, авторизації
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

// ✅ Авторизація — без дублювання схеми
builder.Services.AddAuthorization();

// 🔄 Blazor автентифікація
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();
// 🔄 API контролери
builder.Services.AddControllers(); // для API

builder.Services.AddHttpClient(); // ✅ Це реєструє HttpClient для ін’єкції



// 🧱 Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// 🛠️ Custom services — після Identity
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
// builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<AuthService>();

// 🔧 Логування
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddConsole();
// 🔧 Razor Pages (для помилок)
builder.Services.AddRazorPages();


var app = builder.Build();

// 🔧 Ініціалізація ролей та користувачів
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    await IdentityInitializer.SeedRolesAsync(roleManager);
    await IdentityInitializer.SeedUsersAsync(userManager);
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
app.MapControllers(); // для маршрутизації       // 🔥 після app.UseRouting()


app.UseAuthentication(); // ✅ Обов’язково перед Authorization
app.UseAuthorization();
app.UseAntiforgery();

// 🔗 Маршрутизація компонентів
// app.MapStaticAssets();
app.MapRazorPages(); // Для помилок
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

// 🔧 Ініціалізація бази даних подій

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await EventSeeder.SeedEventsAsync(db);
}




app.Run();
