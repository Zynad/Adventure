# Adventure — D&D Text Adventure Game

## Project Overview
A D&D 5e-inspired text adventure game with an open-world sandbox design. Built as a monorepo with .NET 10 backend and React TypeScript frontend.

## Tech Stack
- **Backend**: .NET 10, ASP.NET Core Web API, EF Core + SQLite, MediatR, FluentValidation
- **Frontend**: React 18+, TypeScript, Vite, Tailwind CSS, Zustand, Node 22+
- **Architecture**: Clean Architecture (Domain → Application → Infrastructure → Api)

## Monorepo Structure
```
src/backend/     - .NET solution (Adventure.slnx)
src/frontend/    - React app (Vite)
assets/data/     - Seed data JSON files
```

## Backend Projects
- `Adventure.Domain` — Entities, Value Objects, Enums, Interfaces, Rules (zero dependencies)
- `Adventure.Application` — Features (CQRS with MediatR), DTOs, Services
- `Adventure.Infrastructure` — EF Core, Repositories, AI implementations, Seeding
- `Adventure.Api` — Controllers, Middleware, DI composition root
- `tests/` — xUnit test projects per layer

## Key Patterns
- **Clean Architecture**: Domain has no dependencies. Application depends only on Domain. Infrastructure implements interfaces. Api is the composition root.
- **CQRS**: Features organized as Commands/Queries with MediatR handlers
- **Repository + Unit of Work**: Generic repository pattern with EF Core
- **Strategy Pattern**: Combat AI uses ICombatAI interface with strategy handlers per AIStrategy enum

## Running the Project

### Development Mode
```bash
# Backend
cd src/backend
dotnet run --project Adventure.Api

# Frontend (separate terminal)
cd src/frontend
npm run dev
```

### Docker (Play Mode)
```bash
docker-compose up --build
```

## Game Design
- **Combat**: D&D 5e rules (d20, AC, spell slots, initiative, turn-based)
- **Classes**: Fighter, Wizard, Rogue, Cleric, Ranger, Paladin (expandable)
- **Races**: PHB 9 (Human, Elf, Dwarf, Halfling, Half-Elf, Half-Orc, Gnome, Dragonborn, Tiefling)
- **World**: World map + zone-based with 2D tile rendering
- **Factions**: Multiple factions with reputation system
- **Professions**: 8 professions (Blacksmithing, Leatherworking, Tailoring, Woodworking, Alchemy, Enchanting, Mining, Herbalism)
- **Admin**: Backoffice at /admin for CRUD on all game content

## Workflow
- Always update TODO.md when completing tasks — check off finished items and add new ones as needed

## Conventions
- Use file-scoped namespaces in C#
- Use record types for DTOs and Value Objects where appropriate
- Entity properties use private setters; create via static factory methods or constructors
- All game logic is server-authoritative
- Frontend is purely a renderer — no game logic on client
- Prefer feature-based folder organization over technical (e.g., Features/Combat/ not Commands/ + Queries/)
- SQLite database file: adventure.db
- NuGet source: uses local nuget.config (nuget.org only)
