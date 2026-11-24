# Music Festival Operations System — COMP1551 Application Development

A C# desktop information system for a 3‑day music festival. It replaces spreadsheets with a
database-backed system for managing the people who deliver the event: **Performers**,
**Crew** and **Vendors**.

It has two user interfaces built on the **same** Business and Data layers:

| UI | Project | Notes |
|----|---------|-------|
| Menu-driven **console** application (required by the brief) | `MVCConsoleApp.Presentation` | MVC: Views + Controllers |
| **Windows Forms** application | `MVCConsoleApp.WindowsForm` | Same services, different presentation |

---

## 1. Architecture (3 tiers + MVC)

```
MVCConsoleApp.Presentation  (Console)   MVCConsoleApp.WindowsForm (WinForms)
   Views/        -> console I/O only        FormPerson / FormLookup / FormReport
   Controllers/  -> menu actions
                 \                         /
                  v                       v
             MVCConsoleApp.BusinessLogic   (Business layer)
                Services/     PersonService, LookupService, ReportService
                Validation/   BusinessRules, FieldRules, PersonValidator (+3 subclasses)
                                    |
                                    v
             MVCConsoleApp.Data            (Data Access layer + Model)
                Entities/     Person (abstract) <- Performer, CrewMember, Vendor
                Repositories/ PersonRepository, LookupRepository, ReportRepository (ALL SQL)
                                    |
                                    v
                         SQL Server 2022 (Docker)
```

Separation rules respected:
* Views contain no SQL and never call the database.
* Controllers contain no SQL; they call the Business layer only.
* All SQL is in `MVCConsoleApp.Data/Repositories` (parameterised commands, transactions).
* Business rules (unique email, exactly 2 genres, exactly 3 categories, weekly hours
  FullTime 25–40 / PartTime 1–24) are enforced in the Business layer **and** backed by
  database constraints.

OOP: encapsulation (private fields + validating setters), inheritance
(`Person` → `Performer`/`CrewMember`/`Vendor`, `PersonValidator` → 3 validators,
`ConsoleView` → 3 views, `RepositoryBase` → 3 repositories) and polymorphism
(`Role`, `DescribeRoleFields()`, `EstimateCost()`, `ValidateRoleFields()`).

---

## 2. Database setup

The database runs in a SQL Server 2022 Docker container on `localhost,1433`.

```bash
# (only once) create the container
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<your-password>" -p 1433:1433 \
  --name sqlserver-container -d mcr.microsoft.com/mssql/server:2022-latest

# start it later with
docker start sqlserver-container
```

Run the two scripts (SSMS, Azure Data Studio, VS SQL Server Object Explorer, or sqlcmd):

1. `Database/databasescheme.sql` — drops & recreates **MusicFestivalDB** (tables, PK/FK, UNIQUE, CHECK).
2. `Database/sampledata.sql` — 4 performers (2 genres each), 4 crew, 3 vendors (3 categories each).

```bash
docker cp Database/databasescheme.sql sqlserver-container:/tmp/
docker cp Database/sampledata.sql    sqlserver-container:/tmp/
docker exec sqlserver-container /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<pwd>" -C -i /tmp/databasescheme.sql
docker exec sqlserver-container /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<pwd>" -C -i /tmp/sampledata.sql
```

The connection string is in `MVCConsoleApp.Data/DbConfig.cs`. It can be overridden without
recompiling by setting the environment variable `FESTIVAL_DB_CONNECTION`.

---

## 3. Running in Visual Studio

1. Open `MVCConsoleApp.slnx`.
2. Right-click **MVCConsoleApp.Presentation** → *Set as Startup Project* → **F5** (console UI),
   or **MVCConsoleApp.WindowsForm** → *Set as Startup Project* → **F5** (Windows Forms UI).
3. Unit tests: *Test → Run All Tests* (37 xUnit tests, no database needed).

From the command line:

```bash
dotnet run --project MVCConsoleApp.Presentation
dotnet run --project MVCConsoleApp.WindowsForm
dotnet test
```

---

## 4. Features

| # | Console menu | Windows Forms |
|---|--------------|---------------|
| 1 | Add a new record (Performer / Crew / Vendor) | Fill the form → **Save** |
| 2 | View all records | Grid (Role = *All roles*) |
| 3 | View by role | Role filter combo |
| 4 | Edit a record by PersonID (Enter keeps a value; role can change) | Click a row → edit → **Save** |
| 5 | Delete by PersonID with confirmation (soft delete) | **Delete** + confirmation |
| 6 | Search by name / email (partial match) | Search box |
| 7 | Sort (ID, name, role, email, estimated cost) | Click a column header |
| 8 | Reports: performers by genre, vendors by category, cost estimates | **Reports...** |
| 9 | Export to CSV (all or one role) | **Export CSV...** |
| 10 | Recycle bin: restore / permanently delete | *Show recycle bin* + Restore / Delete permanently |

Input never crashes the program: menu choices, numbers and decimals are parsed with
`TryParse` and re-asked; type `cancel` at any console prompt to abandon an operation.

---

## 5. Project structure

```
Database/                     databasescheme.sql, sampledata.sql
MVCConsoleApp.Data/           DbConfig, DataAccessException, Entities/, Repositories/
MVCConsoleApp.BusinessLogic/  Common/, Services/, Validation/
MVCConsoleApp.Presentation/   Program.cs, Controllers/, Views/
MVCConsoleApp.WindowsForm/    FormPerson, FormLookup, FormReport
MVCConsoleApp.Tests/          xUnit tests + in-memory fake repository
```
