import { apiClient } from './client';
import type { DialogueNodeDto } from '../types';

export const getNpcDialogue = (npcId: string) =>
  apiClient.get<DialogueNodeDto>(`/npc/${npcId}/dialogue`);

export const advanceDialogue = (npcId: string, currentNodeId: string, chosenOptionIndex: number) =>
  apiClient.post<DialogueNodeDto>(`/npc/${npcId}/dialogue/advance`, {
    npcId,
    currentNodeId,
    chosenOptionIndex,
  });
