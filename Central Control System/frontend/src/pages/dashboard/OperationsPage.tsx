import { useState, useEffect, useRef, useMemo } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';
import { ConfirmDialog, confirmDialog } from 'primereact/confirmdialog';
import axios from 'axios';
import { TokenInfoDTO, UserDTO } from '../../types/authTypes';
import { DetailEvent, EventPaginationQuery, OperationWorkerDTO } from '../../types/eventTypes'; 
import { Toast } from "primereact/toast";
import { ErrorModel } from "../../types/commonTypes";
import { EventStatusDeleted } from '../../types/constants';
import EditOperationDialog from '../../components/EditOperationDialog'; 


export default function OperationsPage() {
  const [events, setEvents] = useState<DetailEvent[]>([]);
  const [totalRecords, setTotalRecords] = useState(0);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [rows, setRows] = useState(5);
  const [selectedEvent, setSelectedEvent] = useState<DetailEvent | null>(null);
  const [editDialogVisible, setEditDialogVisible] = useState(false);
  const toast = useRef<Toast>(null);

  const user: UserDTO | null = useMemo(() => {
    const storedUser = localStorage.getItem('user');
    return storedUser ? JSON.parse(storedUser) : null;
  }, []);

  const operationWorker: OperationWorkerDTO | null = useMemo(() => {
    const storedOperationWorker = localStorage.getItem('operationWorker');
    return storedOperationWorker ? JSON.parse(storedOperationWorker) : null;
  }, []);

  const isAdmin = useMemo(() => user?.roles?.some(role => role.name === 'Admin'), [user]);

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
          eventStatusGID: null,
          coordinatorGID:null,
          dispatcherGID:null
        };
        const response = await axios.post<{ items: DetailEvent[], totalCount: number }>(
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

    } catch (error: any) {
      const apiError = error.response?.data as ErrorModel;
      toast.current?.show({
          severity: "error",
          summary: "Failed to fetch events",
          detail: apiError.message,
          life: 3000
        });
    } finally {
      setLoading(false);
    }
  }

  const showDeleteConfirm = (event: DetailEvent) => {
    confirmDialog({
      message: 'Ви впевнені?',
      header: 'Підтвердження',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Так',
      rejectLabel: 'Ні',
      accept: () => deleteEvent(event.gid)
    });
  };

  const deleteEvent = async (gid: string) => {
    try {
      const tokenInfo = localStorage.getItem('token');
      if (!tokenInfo) return;
      const token = JSON.parse(tokenInfo) as TokenInfoDTO;

      await axios.delete(`${process.env.REACT_APP_API_BASE_URL}/event/${gid}`, {
        headers: { Authorization: `Bearer ${token.token}` },
      });

      toast.current?.show({
        severity: 'success',
        summary: 'Успіх',
        detail: 'Операцію видалено',
        life: 2000,
      });
      fetchEvents(page, rows);
    } catch (err) {
      toast.current?.show({
        severity: 'error',
        summary: 'Помилка',
        detail: 'Не вдалося видалити операцію',
        life: 3000,
      });
    }
  };

  const openEditDialog = (event: DetailEvent) => {
    setSelectedEvent(event);
    setEditDialogVisible(true);
  };

  function onPage(event: any) {
    setPage(event.page);
    setRows(event.rows);
  }
  const canEdit = (event: DetailEvent) => {
    return isAdmin || event.dispatcherGID === operationWorker?.gid;
  };
  const canDelete = (event: DetailEvent) => {
    return canEdit(event) && event.eventStatusGID.toLowerCase() !== EventStatusDeleted.gid.toLowerCase();
  };
  return (
    <div className='operation-table'>
      <h2>Список усіх операцій</h2>
      <Toast ref={toast} />
      <ConfirmDialog style={{color:"rgb(255, 255, 255)"}} />

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
        <Column
          header=""
          body={(rowData: DetailEvent) => (
            <div className="flex gap-2">     
            {canEdit(rowData) && (           
              <Button icon="pi pi-pencil" className="p-button-rounded" style={{backgroundColor: "rgb(85, 147, 240)"}} onClick={() => openEditDialog(rowData)} />
            )}
              {canDelete(rowData) && (
                <Button
                  icon="pi pi-trash"
                  className="p-button-rounded"
                  style={{ backgroundColor: "rgb(192, 48, 48)" }}
                  onClick={() => showDeleteConfirm(rowData)}
                />
              )}             
            </div>
          )}
          style={{ width: '6%' }}
        />
      </DataTable>

      <EditOperationDialog
        selectedEvent={selectedEvent}
        visible={editDialogVisible}
        onHide={() => setEditDialogVisible(false)}
      />
    </div>
  );
}