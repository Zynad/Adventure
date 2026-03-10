import { apiClient } from './client';
import type { Character, CreateCharacterRequest } from '../types';

export const createCharacter = (data: CreateCharacterRequest) =>
  apiClient.post<Character>('/character', data);

export const getCharacter = (id: string) =>
  apiClient.get<Character>(`/character/${id}`);
