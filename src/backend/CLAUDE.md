# Adventure Backend

## Architecture
Clean Architecture with 4 layers:
- **Domain** (innermost) → zero dependencies, pure C#
- **Application** → depends on Domain only, contains MediatR handlers
- **Infrastructure** → depends on Domain + Application, implements interfaces
- **Api** → composition root, depends on Application + Infrastructure (for DI only)

## Running
```bash
dotnet run --project Adventure.Api
```
API runs on http://localhost:5000

## Key Commands
```bash
dotnet build                    # Build all projects
dotnet test                     # Run all tests
dotnet ef migrations add <Name> --project Adventure.Infrastructure --startup-project Adventure.Api
dotnet ef database update --project Adventure.Infrastructure --startup-project Adventure.Api
```

## Solution
Uses the modern `.slnx` format (default in .NET 10+).
```bash
dotnet sln Adventure.slnx list        # List all projects
```

## Conventions
- File-scoped namespaces
- Entities use private setters + static Create factories
- Features organized as vertical slices: Features/{Name}/Commands/ and Queries/
- DTOs are records
- Use FluentValidation for input validation
- Use MediatR pipeline behaviors for cross-cutting concerns
- SQLite database file: adventure.db (in project root during dev)
- Seed data lives in Infrastructure/Persistence/Seeding/SeedData/ as JSON
- NuGet config: uses local nuget.config with nuget.org only

## D&D 5e Rules (SRD)
- Attack: d20 + ability mod + proficiency ≥ AC
- Ability modifier: floor((score - 10) / 2)
- Proficiency: +2 (L1-4), +3 (L5-8), +4 (L9-12), +5 (L13-16), +6 (L17-20)
- Critical hit: natural 20 = double damage dice
- XP table: see Domain/Rules/ExperienceTable.cs
