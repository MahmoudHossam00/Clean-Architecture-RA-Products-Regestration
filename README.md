# RA Products Registration

RA Products Registration is a full-stack regulatory affairs system for managing pharmaceutical product records and their supporting approval data. It centralizes product master data, licenses, technical documents, regulatory deadlines, variations, appeals, promotional materials, miscellaneous approvals, suppliers, and file attachments.

The solution uses a layered .NET backend and an Angular single-page application. It also reads reference data from external CTD and JDE databases.

## Main capabilities

- Browse products and maintain their basic regulatory data.
- Track dosage forms, storage conditions, shelf life, approved packs, prices, manufacturers, and active ingredients.
- Manage product licenses, requirements, departments, categories, and marketing types.
- Store technical documents and approval attachments.
- Record product variations, appeals, promotional materials, and miscellaneous approvals.
- Track PV and Decree 600 deadlines and their history.
- Synchronize products, companies, and active ingredients from CTD.
- Synchronize suppliers from JDE.
- Expose an OpenAPI document and Scalar API reference during development.

## Architecture

The backend follows a layered, domain-oriented design:

| Project | Responsibility |
| --- | --- |
| `RAProductsRegestration.Domain` | Entities, value objects, domain rules, errors, and result types. |
| `RAProductsRegestration.Application` | Use cases, commands, queries, validation, caching behaviors, and integration abstractions. |
| `RAProductsRegestration.Infrastructure` | Entity Framework Core persistence, repositories, migrations, file storage, and CTD/JDE integrations. |
| `RAProductsRegestration.API` | HTTP controllers, request contracts, dependency injection, exception handling, CORS, and OpenAPI. |
| `RAProductsRegestration.Angular/RAProductsRegestration` | Angular user interface, feature pages, API services, routing, and notifications. |

Requests enter through an API controller, are dispatched through MediatR to an application handler, and use domain objects plus infrastructure implementations for persistence or external integrations.

## Technologies

### Backend

- .NET 10 and ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server for application and CTD data
- Oracle Entity Framework Core provider for JDE data
- MediatR for command/query dispatch and pipeline behaviors
- FluentValidation for request validation
- .NET HybridCache
- OpenAPI with Scalar API Reference
- Local file-system attachment storage

### Frontend

- Angular 19
- TypeScript 5.7
- RxJS 7.8
- Angular Router and reactive forms
- ngx-toastr notifications
- Karma and Jasmine for unit tests

## Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0)
- Node.js 20 or later and npm
- SQL Server access for the application and CTD databases
- Oracle database access for the JDE integration
- Valid connection strings for all three databases

## Configuration

Real configuration files are intentionally excluded from Git because they can contain database credentials and internal paths. Create your local settings file from the safe template:

```powershell
Copy-Item RAProductsRegestration.API/appsettings.example.json `
  RAProductsRegestration.API/appsettings.json
```

Then replace the placeholder values in `RAProductsRegestration.API/appsettings.json`:

| Setting | Purpose |
| --- | --- |
| `ConnectionStrings:DefaultConnection` | Main SQL Server database; EF Core migrations target this database. |
| `ConnectionStrings:CTDConnection` | Read-only SQL Server connection used by CTD synchronization. |
| `ConnectionStrings:JDEConnection` | Oracle connection used by supplier synchronization. |
| `AttachmentStorage:Path` | Directory used to store uploaded attachments. |
| `FrontURLs` | Allowed frontend origins for CORS. |

ASP.NET Core environment variables can be used instead of a local file. Use double underscores for nested keys, for example `ConnectionStrings__DefaultConnection` and `AttachmentStorage__Path`.

> Never commit passwords, tokens, private keys, production connection strings, or real `appsettings` files. If a secret was previously shared or committed, rotate it; adding it to `.gitignore` does not remove it from history.

## Run the backend

From the repository root:

```powershell
dotnet restore RAProductsRegestration.sln
dotnet run --project RAProductsRegestration.API
```

The development launch profile uses:

- HTTPS: `https://localhost:7297`
- HTTP: `http://localhost:5277`
- Scalar API reference: `https://localhost:7297/scalar/v1`
- OpenAPI document: `https://localhost:7297/openapi/v1.json`

The application applies pending EF Core migrations to the main database during startup. Make sure the database is reachable and the configured account can apply schema changes.

If the local HTTPS certificate is not trusted, run:

```powershell
dotnet dev-certs https --trust
```

## Run the frontend

The Angular workspace is nested one level below the frontend wrapper directory:

```powershell
Set-Location RAProductsRegestration.Angular/RAProductsRegestration
npm ci
npm start
```

Open `http://localhost:4200`. The frontend currently targets `https://localhost:7297`, matching the API development profile. That value is defined in `src/app/core/config/api.config.ts`.

## Build and test

Build the backend:

```powershell
dotnet build RAProductsRegestration.sln
```

Build and test the frontend:

```powershell
Set-Location RAProductsRegestration.Angular/RAProductsRegestration
npm run build
npm test
```

## Database migrations

Migration files live in `RAProductsRegestration.Infrastructure/Migrations`. From the repository root, create a migration with:

```powershell
dotnet ef migrations add <MigrationName> `
  --project RAProductsRegestration.Infrastructure `
  --startup-project RAProductsRegestration.API
```

The API applies pending migrations automatically when it starts. You can also apply them explicitly:

```powershell
dotnet ef database update `
  --project RAProductsRegestration.Infrastructure `
  --startup-project RAProductsRegestration.API
```

## Security notes

- Keep local configuration and secret files outside version control.
- Prefer environment variables or an approved secret manager in deployed environments.
- Do not place uploaded regulatory documents under source control.
- Review CORS origins and database permissions before deployment.
- Run a secret scan before publishing changes to a remote repository.

