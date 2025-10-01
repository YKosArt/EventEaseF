# EventEaseF
Events App
# EventEaseF

**EventEaseF** — це Blazor Server App для управління подіями з реєстрацією учасників, категоріями, фільтрами, статусами та інтерактивним UI. Побудовано на .NET 8 з EF Core, DI, та чистою архітектурою.

## 🔥 Особливості

- Реєстрація/скасування участі в подіях
- Фільтрація: майбутні, минулі, доступні, заповнені
- Авторизація через ASP.NET Identity
- EF Core-backed логіка без in-memory хакасів
- Динамічні компоненти Blazor з централізованим станом

## 🚀 Онлайн демо

👉 [https://eventeasef.onrender.com](https://eventeasef.onrender.com)  
(автоматичний деплой з гілки `main` через Render.com)

## 🛠️ Запуск локально

### Вимоги

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- SQLite або PostgreSQL (за бажанням)
- Visual Studio / VS Code

### Команди

```bash
git clone https://github.com/YKosArt/EventEaseF.git
cd EventEaseF
dotnet run
