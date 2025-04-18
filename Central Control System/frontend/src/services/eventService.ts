import axios from 'axios';
import { EventDTO } from '../types/eventTypes';

export interface PaginationQuery {
    pageNumber: number;
    pageSize: number;
}

export interface PaginatedResult<T> {
    items: T[];
}

export async function getEvents(
    query: PaginationQuery,
    token: string
): Promise<PaginatedResult<EventDTO>> {
    const response = await axios.get(`${process.env.API_BASE_URL}/event`, {
        params: query,
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
    return response.data;
}
