import { apiClient } from './client';
import type { GameStateDto } from '../types';

export const getGameState = (characterId: string) =>
  apiClient.get<GameStateDto>(`/game/state/${characterId}`);
