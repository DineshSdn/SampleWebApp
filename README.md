# SampleWebApp

This project is a sample web application built using ASP.NET Core, demonstrating a Clean Architecture approach.

## Clean Architecture

The solution is structured based on the principles of Clean Architecture, which promotes separation of concerns, testability, and maintainability. This is achieved by dividing the application into several layers:

*   **Domain:** Contains the core business logic and entities. It has no dependencies on other layers.
*   **Application Core (App.Core & App.Common):**
    *   `App.Core`: Defines interfaces and implements application-specific business rules (use cases). It orchestrates the data flow between the Domain and Infrastructure layers.
    *   `App.Common`: Provides common utilities, abstractions (like for database drivers, security, etc.), and shared models (DTOs, exceptions) used by `App.Core` and `Infrastructure`.
*   **Infrastructure:** Implements external concerns such as data access (using Entity Framework Core and Dapper), file systems, and integrations with third-party services. It depends on the Application Core.
*   **Presentation (WebUI):** The user interface layer. In this case, it's an ASP.NET Core Web API that exposes endpoints to interact with the application. It depends on the Infrastructure layer.

## Project Structure Overview

The solution is organized into the following main project directories:

*   `01-Domain/Domain/`: Contains domain entities and core business logic.
*   `02-Core/App.Common/`: Contains common utilities, abstractions, and models.
*   `02-Core/App.Core/`: Contains application services, interfaces, and DTOs.
*   `03-Infrastructure/Infrastructure/`: Contains implementations for data access, external services, etc.
*   `04-Presentation/WebUI/`: Contains the ASP.NET Core Web API project.

## Getting Started

### Prerequisites

*   .NET 8.0 SDK (or later)

### Building and Running the Application

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    cd <repository-directory>
    ```
2.  **Restore dependencies:**
    Navigate to the solution root directory (where `SampleWebApp.sln` is located) and run:
    ```bash
    dotnet restore SampleWebApp.sln
    ```
3.  **Build the solution:**
    ```bash
    dotnet build SampleWebApp.sln --configuration Release
    ```
4.  **Run the WebUI project:**
    Navigate to the `04-Presentation/WebUI` directory:
    ```bash
    cd 04-Presentation/WebUI
    ```
    Then run the application:
    ```bash
    dotnet run
    ```
    The API will typically be available at `https://localhost:port` or `http://localhost:port` as specified in the `launchSettings.json` file. You can access the Swagger UI at `/swagger`.

    **Note on Database:** This project uses Entity Framework Core.
    *   The connection string is typically configured in `04-Presentation/WebUI/appsettings.json` (and `appsettings.Development.json`).
    *   Migrations are located in `03-Infrastructure/Infrastructure/Migrations/`. To apply migrations if you're setting up the database for the first time:
        ```bash
        cd 03-Infrastructure/Infrastructure
        dotnet ef database update --startup-project ../../04-Presentation/WebUI/WebUI.csproj --project ./Infrastructure.csproj
        ```
        Ensure the database server is running and accessible.

## Technologies and Libraries

This project utilizes a range of .NET technologies and libraries:

*   **ASP.NET Core 8:** For building the Web API (Presentation layer).
*   **.NET 8:** The underlying framework version.
*   **Entity Framework Core 9:** Used as the primary ORM for data access with a SQL Server provider.
    *   Includes EF Core Tools for migrations and database management.
*   **Dapper:** A micro-ORM used for performance-critical database queries or when more control over SQL is needed (available in Infrastructure).
*   **Mapster:** For object-to-object mapping, helping to convert entities to DTOs and vice-versa.
*   **Newtonsoft.Json:** For JSON serialization and deserialization tasks.
*   **Swashbuckle.AspNetCore:** To generate Swagger/OpenAPI documentation for the Web API, providing an interactive UI for testing endpoints.
*   **Dependency Injection:** Leverages ASP.NET Core's built-in DI container, with services registered in `DependencyInjection.cs` files within `App.Core` and `Infrastructure` layers.

## Detailed Project Structure

The solution adheres to Clean Architecture principles, with a clear separation of concerns across different projects:

```
SampleWebApp/
├── 01-Domain/
│   └── Domain/                     # Core business logic and entities
│       ├── Common/
│       │   └── AuditableEntity.cs  # Base class for entities with audit properties
│       ├── Entities/               # Domain entities (e.g., Employee.cs, Department.cs)
│       └── Domain.csproj
├── 02-Core/
│   ├── App.Common/                 # Shared utilities, abstractions, and base classes
│   │   ├── Abstractions/           # Interfaces for common services (DB drivers, security, utilities)
│   │   ├── Adapters/               # Adapters for external services (logging, JSON, data)
│   │   ├── Attributes/             # Custom attributes (e.g., for DI service lifetimes)
│   │   ├── Constants/
│   │   ├── Exceptions/
│   │   ├── Extensions/
│   │   ├── Helpers/
│   │   ├── Models/                 # Common DTOs (AppResponseDto, PageFilterDto)
│   │   ├── Validators/             # Validation logic and infrastructure
│   │   └── App.Common.csproj
│   └── App.Core/                   # Application logic, use cases
│       ├── Abstractions/           # Interfaces for application services (e.g., IEmployeeService.cs)
│       ├── Models/                 # Application-specific DTOs (e.g., EmployeeDto.cs)
│       ├── Services/               # Implementation of application services (e.g., EmployeeService.cs)
│       ├── DependencyInjection.cs  # Service registration for this layer
│       └── App.Core.csproj
├── 03-Infrastructure/
│   └── Infrastructure/             # Data access, external service implementations
│       ├── Adapters/               # Concrete implementations of App.Common adapters
│       ├── Implementations/        # Concrete implementations of App.Common abstractions (DB drivers, utilities)
│       │   └── DbDrivers/
│       │       └── AppDbContext.cs # Entity Framework DbContext
│       ├── Migrations/             # EF Core database migrations
│       ├── DependencyInjection.cs  # Service registration for this layer
│       └── Infrastructure.csproj
├── 04-Presentation/
│   └── WebUI/                      # ASP.NET Core Web API project
│       ├── Controllers/            # API controllers (e.g., EmployeesController.cs)
│       ├── Properties/
│       │   └── launchSettings.json # IIS/Kestrel launch profiles
│       ├── appsettings.json        # Application configuration
│       ├── appsettings.Development.json # Development-specific configuration
│       ├── Program.cs              # Main entry point, service configuration, request pipeline
│       └── WebUI.csproj
└── SampleWebApp.sln                # Solution file
```

### Key Components:

*   **Domain Entities (`01-Domain/Domain/Entities/`)**: Plain C# objects representing core business concepts (e.g., `Employee`, `Department`).
*   **Application Services (`02-Core/App.Core/Services/`)**: Implement the use cases of the application (e.g., `EmployeeService`). They use interfaces defined in `02-Core/App.Core/Abstractions/`.
*   **Common Abstractions & Utilities (`02-Core/App.Common/`)**: Provides reusable components like `IDbConnectionFactory`, `ILoggerAdapter`, DTOs like `AppResponseDto`, and exception classes.
*   **Data Access (`03-Infrastructure/Infrastructure/`)**:
    *   `AppDbContext.cs` (located in `03-Infrastructure/Infrastructure/Implementations/DbDrivers/`): The Entity Framework Core DbContext.
    *   Repositories (if used) would also reside here, implementing interfaces defined in `App.Core`.
    *   Database migrations are managed here.
*   **API Controllers (`04-Presentation/WebUI/Controllers/`)**: Handle HTTP requests, mediate between the client and application services.
*   **Dependency Injection (`DependencyInjection.cs` in `App.Core` and `Infrastructure`, `Program.cs` in `WebUI`)**: Configures how services and their dependencies are resolved.
