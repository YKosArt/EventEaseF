# EventEaseF — Events Management App

**EventEaseF** is a Blazor Server App for managing events with participant registration, categories, filters, statuses, and an interactive UI. Built on **.NET 8**, using **EF Core**, **Dependency Injection**, and clean architecture.

---

## 🔥 Features

- ✅ Register or cancel participation in events  
- 🔍 Filter events: upcoming, past, available, full  
- 🔐 Authorization via ASP.NET Identity  
- 🧠 EF Core-backed logic — no in-memory hacks  
- ⚡ Dynamic Blazor components with centralized state

---

## 🚀 Live Demo

👉 [eventeasef.onrender.com](https://eventeasef.onrender.com)  
Automatic deployment from the `main` branch via [Render.com](https://render.com)

---

## 🛠️ Run Locally

### Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)  
- SQLite or PostgreSQL (optional)  
- Visual Studio / VS Code

### Commands

```bash
git clone https://github.com/YKosArt/EventEaseF.git
cd EventEaseF
dotnet run

EventEaseF/
├── Components/         # Razor components
├── Data/               # EF Core DbContext
├── Models/             # Domain models
├── Services/           # DI services
├── Pages/              # Blazor pages
├── wwwroot/            # Static assets
└── Program.cs          # App startup

🧪 Status
✅ Production-ready
🧪 Public testing via Render
📈 CI/CD via GitHub + Docke

🙌 Author
Yurii Kostiuk — Full-stack web developer & architect
Building maintainable, minimalist platforms with modern .NET stack


```
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
