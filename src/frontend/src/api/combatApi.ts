import { apiClient } from './client';
import type { CombatStateDto, CombatActionResultDto, CombatConsumableDto } from '../types';

export const initiateCombat = (characterId: string, monsterIds: string[]) =>
  apiClient.post<CombatStateDto>('/combat/initiate', { characterId, monsterIds });

export const executeAttack = (characterId: string, targetId: string) =>
  apiClient.post<CombatActionResultDto>('/combat/attack', { characterId, targetId });

export const executeCombatAction = (characterId: string, actionType: number, targetId?: string) =>
  apiClient.post<CombatActionResultDto>('/combat/action', { characterId, actionType, targetId });

export const endTurn = (characterId: string) =>
  apiClient.post<CombatStateDto>('/combat/end-turn', { characterId });

export const useItemInCombat = (characterId: string, itemId: string) =>
  apiClient.post<CombatActionResultDto>('/combat/use-item', { characterId, itemId });

export const getCombatConsumables = (characterId: string) =>
  apiClient.get<CombatConsumableDto[]>(`/combat/consumables/${characterId}`);

export const getCombatState = (characterId: string) =>
  apiClient.get<CombatStateDto>(`/combat/state/${characterId}`);
