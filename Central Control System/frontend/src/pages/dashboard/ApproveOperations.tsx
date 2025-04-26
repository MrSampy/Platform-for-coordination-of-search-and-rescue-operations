// Enhancement of your page with comment modal

import { useState, useEffect, useRef } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import axios from 'axios';
import { TokenInfoDTO } from '../../types/authTypes';
import { DetailEvent, EventPaginationQuery, EventStatusChangeRequest } from '../../types/eventTypes';
import { EventStatusCreated, EventStatusApproved, EventStatusRejected } from '../../types/constants';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast';
import { Dialog } from 'primereact/dialog';
import { InputTextarea } from 'primereact/inputtextarea';
import { ErrorModel } from '../../types/commonTypes';

export default function ApproveOperations() {
  const [events, setEvents] = useState<DetailEvent[]>([]);
  const [totalRecords, setTotalRecords] = useState(0);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [rows, setRows] = useState(5);
  const toast = useRef<Toast>(null);

  const [dialogVisible, setDialogVisible] = useState(false);
  const [note, setNote] = useState('');
  const [selectedEvent, setSelectedEvent] = useState<DetailEvent | null>(null);
  const [targetStatus, setTargetStatus] = useState<string>('');

  useEffect(() => {
    fetchEvents(page, rows);
  }, [page, rows]);

  async function fetchEvents(page: number, rows: number) {
    try {
      setLoading(true);
      const tokenInfo = localStorage.getItem('token');
      if (tokenInfo !== null) {
        const token = JSON.parse(tokenInfo) as TokenInfoDTO;
        const paginationQuery: EventPaginationQuery = {
          pageNumber: page + 1,
          pageSize: rows,
          eventStatusGID: EventStatusCreated.gid,
          coordinatorGID:null,
          dispatcherGID:null
        };
        const response = await axios.post<{ items: DetailEvent[]; totalCount: number }>(
          `${process.env.REACT_APP_API_BASE_URL}/event/sort`,
          paginationQuery,
          {
            headers: {
              Authorization: `Bearer ${token.token}`
            }
          }
        );
        setEvents(response.data.items);
        setTotalRecords(response.data.totalCount);
      }
    } catch (error: any) {
      const apiError = error.response?.data as ErrorModel;
      toast.current?.show({
        severity: 'error',
        summary: 'Failed to fetch events',
        detail: apiError.message,
        life: 3000
      });
    } finally {
      setLoading(false);
    }
  }

  function onPage(event: any) {
    setPage(event.page);
    setRows(event.rows);
  }

  function openStatusDialog(event: DetailEvent, status: string) {
    setSelectedEvent(event);
    setTargetStatus(status);
    setNote('');
    setDialogVisible(true);
  }

  async function confirmStatusChange() {
    if (!selectedEvent) return;
    try {
      const tokenInfo = localStorage.getItem('token');
      if (tokenInfo !== null) {
        const token = JSON.parse(tokenInfo) as TokenInfoDTO;
        const request: EventStatusChangeRequest = {
          eventGID: selectedEvent.gid,
          eventStatusGID: targetStatus,
          note: note
        };
        await axios.post(`${process.env.REACT_APP_API_BASE_URL}/event/status-change`, request, {
          headers: {
            Authorization: `Bearer ${token.token}`
          }
        });
        setDialogVisible(false);
        fetchEvents(page, rows);
        toast.current?.show({
          severity: 'success',
          summary: 'Статус оновлено',
          detail: 'Подія успішно оновлена',
          life: 2000
        });
      }
    } catch (error: any) {
      const apiError = error.response?.data as ErrorModel;
      toast.current?.show({
        severity: 'error',
        summary: 'Помилка',
        detail: apiError.message,
        life: 3000
      });
    }
  }

  return (
    <div className="border-round-xl shadow-2 p-4" style={{ backgroundColor: 'white' }}>
      <Toast ref={toast} />
      <h2>Оберіть операцію для погодження</h2>
      <div className="operation-table">
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
          <Column field="dispatcher" header="Диспетчер" style={{ width: '20%' }} />
          <Column field="coordinator" header="Координатор" style={{ width: '20%' }} />
          <Column
            body={(rowData: DetailEvent) => (
              <div className="flex gap-2">
                  <Button
                  raised
                  className='p-button-rounded'
                  icon = 'pi pi-check'
                  severity="warning"
                  style={{ backgroundColor: 'rgb(184, 226, 164)' }}
                  onClick={() => openStatusDialog(rowData, EventStatusApproved.gid)}
                />
                <Button
                  raised
                  className='p-button-rounded'
                  icon = 'pi pi-times'
                  severity="warning"
                  style={{ backgroundColor: 'rgb(226, 164, 164)' }}
                  onClick={() => openStatusDialog(rowData, EventStatusRejected.gid)}
                />
              </div>
            )}
            style={{ width: '6%' }}
          />          
        </DataTable>
      </div>

      <Dialog
        header="Коментар до статусу"
        visible={dialogVisible}
        style={{ width: '50vw' }}
        modal
        onHide={() => setDialogVisible(false)}
        footer={
          <div className="flex justify-content-end gap-2">            
            <Button label="Підтвердити" icon="pi pi-check" onClick={confirmStatusChange} autoFocus />
            <Button label="Скасувати" icon="pi pi-times" onClick={() => setDialogVisible(false)} />
          </div>
        }
      >
        <label htmlFor="note">Коментар</label>
        <InputTextarea id="note" value={note} onChange={(e) => setNote(e.target.value)} rows={4} className="w-full" autoFocus />
      </Dialog>
    </div>
  );
}