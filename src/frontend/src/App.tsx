import { BrowserRouter, Routes, Route, useNavigate } from 'react-router-dom';
import { CharacterCreationPage } from './features/character/CharacterCreationPage';
import { GamePage } from './features/game/GamePage';

function HomePage() {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen bg-gray-900 text-white flex items-center justify-center">
      <div className="text-center">
        <h1 className="text-6xl font-bold mb-4">Adventure</h1>
        <p className="text-xl text-gray-400 mb-8">A D&D-Inspired Text Adventure</p>
        <div className="space-x-4">
          <button
            onClick={() => navigate('/create-character')}
            className="px-6 py-3 bg-amber-700 hover:bg-amber-600 rounded-lg text-lg font-semibold transition-colors"
          >
            New Game
          </button>
          <button className="px-6 py-3 bg-gray-700 hover:bg-gray-600 rounded-lg text-lg font-semibold transition-colors">
            Load Game
          </button>
        </div>
      </div>
    </div>
  );
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/create-character" element={<CharacterCreationPage />} />
        <Route path="/game" element={<GamePage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
