# Project Management Portal

**A project & financial management portal for EPC / construction organizations**, built on ASP.NET Core and the ABP Framework.

PMPortal centralizes everything a project-driven engineering or construction company needs to track day to day: active projects, contractors, payment requests, receipts, progress percentages, cost/benefit reports, warranty balances, and document control — all behind role-based access control and multi-tenancy.

> **Origin** — This system was developed as part of my role as **.NET Developer at
> Pishronet**, delivering a project & financial management portal for **MSVCO**, an oil/gas &
> petrochemical client, supporting 200+ contractors. It was pushed to GitHub afterwards as
> a public record of the work.

---

## Features

**Project Management**
- Project lifecycle tracking (type, status, contract value, site, key dates, contract period)
- Financial statements, reports, plans, media/photo galleries, and asset files per project
- Per-user project assignment (users only see the projects they're mapped to)

**Financial Management**
- Payment requests with a multi-stage approval queue (project manager → planning → finance)
- Contractor registry and receive bills / receipts
- Cost & benefit tracking, gross-profit-to-net reporting, warranty balance tracking
- Remaining-credit and transaction ledgers, Excel import/export throughout

**Progress & Resource Tracking**
- Engineering / procurement / execution progress percentages, planned vs. actual, by project and period
- Monthly labor/workforce inventory per project

**Document Control**
- Project task and document revision tracking (discipline, document type, status, transmittal history)
- Bulk import from Excel for legacy document registers

**Dashboards**
- Engineering, Management, Allocation, and Projects dashboards with interactive charts (Chart.js + Kendo UI grids)
- Company-wide and per-project views

**Platform**
- Role-based authorization and multi-tenancy (built on [ABP Framework](https://aspnetboilerplate.com/) 4.9)
- JWT + cookie authentication, localization-ready UI
- RTL-first layout with a modern slate/teal theme

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 2.2 (MVC + Web API host), C# |
| Application framework | [ABP Framework](https://aspnetboilerplate.com/) (Abp.AspNetCore, Abp.ZeroCore) |
| Data access | Entity Framework Core 2.2, SQL Server |
| Frontend | Razor views, Bootstrap, Materialize CSS, Kendo UI grids, Chart.js |
| Auth | ASP.NET Identity, JWT Bearer, cookie auth, multi-tenancy |
| Excel I/O | EPPlus, ExcelDataReader |

---

## Architecture

The solution follows ABP's layered module structure:

```
Nexora.PMPortal.Core                 domain entities, enums, authorization definitions
Nexora.PMPortal.Application          application services, DTOs, business logic
Nexora.PMPortal.EntityFrameworkCore  EF Core DbContext, migrations, repositories
Nexora.PMPortal.Web.Core             shared web infrastructure, auth controllers
Nexora.PMPortal.Web.Host             standalone API host (JWT-secured)
Nexora.PMPortal.Web.Mvc              the main MVC application (Razor views, dashboards)
Nexora.PMPortal.Migrator             console app that applies EF Core migrations
```

---

## Getting Started

### Prerequisites

- [.NET Core SDK 2.2](https://dotnet.microsoft.com/download/dotnet/2.2) (or a newer SDK with 2.2 runtime support)
- SQL Server (LocalDB, Express, or full) running locally
- Windows (some legacy dependencies, e.g. `Microsoft.Office.Interop.Excel`, are Windows-only)

### 1. Clone and restore

```bash
git clone https://github.com/Sina-Ghiabi/PMPortal.git
cd PMPortal
dotnet restore Nexora.PMPortal.sln
```

### 2. Configure the database connection

Edit the connection string in `src/Nexora.PMPortal.Web.Mvc/appsettings.json` (and `src/Nexora.PMPortal.Migrator/appsettings.json`) to point at your local SQL Server instance. The default is already set up for a local `SQLEXPRESS` instance:

```json
"ConnectionStrings": {
  "Default": "Server=localhost\\SQLEXPRESS; Database=PMPortalDb; Trusted_Connection=True;"
}
```

### 3. Apply migrations

```bash
dotnet run --project src/Nexora.PMPortal.Migrator
```

This creates the database and seeds the default tenant, roles, and an initial admin user.

### 4. Run the app

```bash
dotnet run --project src/Nexora.PMPortal.Web.Mvc
```

Then open `http://localhost:5000` (or the port shown in the console) and sign in with the seeded admin account.

> **Note:** this repository ships with no business data. Projects, contractors, financial records, dashboards, etc. will all be empty until you create your own test data through the UI.

---

## Project Structure

```
PMPortal/
├── Nexora.PMPortal.sln
├── NuGet.Config
├── lib/                    salvaged third-party binaries referenced directly (Kendo UI)
├── src/
│   ├── Nexora.PMPortal.Core/
│   ├── Nexora.PMPortal.Application/
│   ├── Nexora.PMPortal.EntityFrameworkCore/
│   ├── Nexora.PMPortal.Migrator/
│   ├── Nexora.PMPortal.Web.Core/
│   ├── Nexora.PMPortal.Web.Host/
│   └── Nexora.PMPortal.Web.Mvc/
└── test/
    └── Nexora.PMPortal.Tests/
```

---

## License

This system was built as client work at Pishronet and is published here as a record of that work. All rights reserved — feel free to browse the code for reference, but please don't redistribute or reuse it without permission.
