import { apiClient } from './client';
import type { MoveResultDto } from '../types';

export const moveCharacter = (characterId: string, dx: number, dy: number) =>
  apiClient.post<MoveResultDto>('/movement/move', { characterId, dx, dy });
