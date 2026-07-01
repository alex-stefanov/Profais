# Profais

Profais is an ASP.NET Core business management application for project requests, projects, tasks, workers, specialists, materials, and penalties. It was built as a practical management system for Profix and as a final-year C# Web project.

## Features

- Client project-request submission
- Project creation, tracking, recovery, and management
- Task assignment and daily task views
- Worker and specialist request workflows
- Material management
- Penalty management and user penalty tracking
- Role-based user management
- Email sender service
- Pagination and shared view models
- Service-layer unit tests

## Tech Stack

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Razor Views
- xUnit-style test project

## Solution Structure

```text
Profais/                 # Web application
Profais.Common/          # Constants, options, enums, exceptions
Profais.Data/            # EF Core context, models, migrations
Profais.Services/        # Services, interfaces, and view models
Profais.Services.Tests/  # Service tests
Profais.sln
```

## Getting Started

### Prerequisites

- .NET SDK
- SQL Server or SQL Server LocalDB

### Run Locally

```bash
git clone https://github.com/alex-stefanov/Profais.git
cd Profais
dotnet restore Profais.sln
dotnet build Profais.sln
dotnet run --project Profais
```

Configure the database connection string and SMTP settings in `appsettings.json`, user secrets, or environment variables before running production-like flows.

## Testing

```bash
dotnet test Profais.sln
```

## License

This project is licensed under the MIT License.
