import { apiClient } from './client';
import type { ZoneDto, TileDto } from '../types';

export const getZones = () =>
  apiClient.get<ZoneDto[]>('/map/zones');

export const getZoneTiles = (zoneId: string) =>
  apiClient.get<TileDto[]>(`/map/zones/${zoneId}/tiles`);
