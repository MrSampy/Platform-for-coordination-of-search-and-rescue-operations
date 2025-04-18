import React, { useState, useEffect } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import axios from 'axios';
import { TokenInfoDTO } from '../../types/authTypes';
import { ClearEvent } from '../../types/eventTypes'; 

export interface EventPaginationQuery {
  pageNumber:number;
  pageSize:number;
}

export default function OperationsPage() {
  const [events, setEvents] = useState<ClearEvent[]>([]);
  const [totalRecords, setTotalRecords] = useState(0);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [rows, setRows] = useState(5);

  useEffect(() => {
    fetchEvents(page, rows);
  }, [page, rows]);

  async function fetchEvents(page: number, rows: number) {
    try {
      setLoading(true);
      const tokenInfo = localStorage.getItem('token');
      if(tokenInfo !== null){
        const token = JSON.parse(tokenInfo) as TokenInfoDTO;
        const paginationQuery: EventPaginationQuery = {
          pageNumber: page + 1,
          pageSize: rows,
        };
        const response = await axios.post<{ items: ClearEvent[], totalCount: number }>(
          `${process.env.REACT_APP_API_BASE_URL}/event/sort`,
          paginationQuery,
          {
            headers: {
              Authorization: `Bearer ${token.token}`,
            },
          }
        );
      setEvents(response.data.items);
      setTotalRecords(response.data.totalCount);
      }

    } catch (error) {
      console.error('Failed to fetch events', error);
    } finally {
      setLoading(false);
    }
  }
  function onPage(event: any) {
    setPage(event.page);
    setRows(event.rows);
  }

  return (
    <div className='operation-table'>
      <DataTable
        stripedRows 
        value={events}
        paginator
        rows={rows}
        lazy
        first={page * rows}
        totalRecords={totalRecords}
        onPage={onPage}
        loading={loading}
        rowsPerPageOptions={[5, 10, 25, 50]}
        tableStyle={{ minWidth: '90rem' }}
      >
        <Column field="name" header="Назва" style={{ width: '10%' }} />
        <Column field="longitude" header="Довгота" style={{ width: '5%' }} />
        <Column field="latitude" header="Широта" style={{ width: '5%' }} />
        <Column field="eventStatus" header="Статус" style={{ width: '10%' }} />
        <Column field="eventType" header="Тип події" style={{ width: '10%' }} />
        <Column field="district" header="Район" style={{ width: '10%' }} />
        <Column field="dispatcher" header="Диспетчер" style={{ width: '25%' }} />
        <Column field="coordinator" header="Координатор" style={{ width: '25%' }} />
      </DataTable>
    </div>
  );
}

