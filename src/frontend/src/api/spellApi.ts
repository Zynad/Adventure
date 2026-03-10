import { apiClient } from './client';
import type { CharacterSpellInfoDto, KnownSpellDto } from '../types';

export const getKnownSpells = (characterId: string) =>
  apiClient.get<CharacterSpellInfoDto>(`/spell/known/${characterId}`);

export const learnSpell = (characterId: string, spellId: string) =>
  apiClient.post<KnownSpellDto>('/spell/learn', { characterId, spellId });
