# Adventure Frontend

## Tech Stack
- React 18+ with TypeScript
- Vite (build tool)
- Tailwind CSS (styling)
- Zustand (state management)
- React Router (routing)
- Axios (API client)
- Node 22+

## Running
```bash
npm run dev     # Start dev server (http://localhost:5173)
npm run build   # Production build
npm run preview # Preview production build
```

## Project Structure
```
src/
  api/          - Axios API client and endpoint wrappers
  components/   - Shared UI components
  features/     - Feature-based modules (game, character, combat, inventory, map, quests, crafting, shop, admin)
  hooks/        - Custom React hooks
  store/        - Zustand stores (gameStore, combatStore, uiStore)
  types/        - TypeScript interfaces matching backend DTOs
  styles/       - Global styles
```

## Conventions
- Functional components with hooks only
- Feature-based folder organization
- Types/interfaces in src/types/ — must match backend DTOs
- API client in src/api/client.ts — base URL from VITE_API_URL env var
- Zustand for global state, React state for local component state
- Tailwind CSS utility classes — no CSS modules or styled-components
- All game logic lives on the backend — frontend is purely a renderer

## Key Routes
```
/                     - Landing page (New Game / Load Game)
/create-character     - Character creation wizard
/game                 - Main game view
/game/combat          - Combat overlay
/game/inventory       - Inventory overlay
/game/quests          - Quest log
/game/crafting        - Crafting panel
/game/shop/:shopId    - Shop view
/game/character-sheet - Character sheet
/admin                - Admin dashboard
/admin/:entity        - Admin CRUD for entity
```

## Environment Variables
- `VITE_API_URL` — Backend API base URL (default: http://localhost:5000)
