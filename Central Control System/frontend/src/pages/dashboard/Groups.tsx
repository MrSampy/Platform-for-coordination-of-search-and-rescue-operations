import { useState, useEffect, useRef, useMemo, useCallback } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast';
import axios from 'axios';
import { TokenInfoDTO, UserDTO } from '../../types/authTypes';
import { DetailGroup, GroupPaginationQuery, GetGroupsByDispatcherGIDRequest } from '../../types/groupTypes'; 
import { OperationWorkerDTO } from '../../types/eventTypes';
import { ErrorModel } from '../../types/commonTypes';
import GroupVolunteersDialog from '../../components/GroupVolunteersDialog';
import GroupTasksDialog from '../../components/GroupTasksDialog';     
export default function GroupsPage() {
  const [groups, setGroups] = useState<DetailGroup[]>([]);
  const [totalRecords, setTotalRecords] = useState(0);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [rows, setRows] = useState(5);
  const toast = useRef<Toast>(null);

  const [volunteersDialogVisible, setVolunteersDialogVisible] = useState(false);
  const [tasksDialogVisible, setTasksDialogVisible] = useState(false);
  const [selectedGroup, setSelectedGroup] = useState<DetailGroup | null>(null);

  const user: UserDTO | null = useMemo(() => {
    const storedUser = localStorage.getItem('user');
    return storedUser ? JSON.parse(storedUser) : null;
  }, []);

  const operationWorker: OperationWorkerDTO | null = useMemo(() => {
    const storedOperationWorker = localStorage.getItem('operationWorker');
    return storedOperationWorker ? JSON.parse(storedOperationWorker) : null;
  }, []);

  const isAdmin = useMemo(() => user?.roles?.some(role => role.name === 'Admin'), [user]);

const fetchGroups = useCallback(async (page: number, rows: number) => {
  try {
    setLoading(true);
    const tokenInfo = localStorage.getItem('token');
    if (tokenInfo !== null) {
      const token = JSON.parse(tokenInfo) as TokenInfoDTO;
      let groupsRes: DetailGroup[] = [];
      let totalCount: number = 0;
      if (isAdmin) {
        const paginationQuery: GroupPaginationQuery = {
          pageNumber: page + 1,
          pageSize: rows,
          leaderGID: null,
          eventGID: null
        };
        const response = await axios.post<{ items: DetailGroup[], totalCount: number }>(
          `${process.env.REACT_APP_API_BASE_URL}/group/sort`,
          paginationQuery,
          { headers: { Authorization: `Bearer ${token.token}` } }
        );
        groupsRes = response.data.items;
        totalCount = response.data.totalCount;
      } else {
        const paginationQuery: GetGroupsByDispatcherGIDRequest = {
          pageNumber: page + 1,
          pageSize: rows,
          dispatcherGID: operationWorker?.gid
        };
        const response = await axios.post<{ items: DetailGroup[], totalCount: number }>(
          `${process.env.REACT_APP_API_BASE_URL}/group/by-dispatcher`,
          paginationQuery,
          { headers: { Authorization: `Bearer ${token.token}` } }
        );
        groupsRes = response.data.items;
        totalCount = response.data.totalCount;
      }

      setGroups(groupsRes);
      setTotalRecords(totalCount);
    }
  } catch (error: any) {
    const apiError = error.response?.data as ErrorModel;
    toast.current?.show({
      severity: 'error',
      summary: 'Помилка завантаження груп',
      detail: apiError.message,
      life: 3000
    });
  } finally {
    setLoading(false);
  }
}, [isAdmin, operationWorker]); // Додали залежності сюди

useEffect(() => {
    fetchGroups(page, rows);
  }, [page, rows, fetchGroups]); 

  function onPage(event: any) {
    setPage(event.page);
    setRows(event.rows);
  }

  function openVolunteersDialog(group: DetailGroup) {
    setSelectedGroup(group);
    setVolunteersDialogVisible(true);
  }

  function openTasksDialog(group: DetailGroup) {
    setSelectedGroup(group);
    setTasksDialogVisible(true);
  }

  const actionTemplate = (rowData: DetailGroup) => (
    <div className="flex gap-2 justify-content-center">
      <Button
        icon="pi pi-users"
        severity="warning"
        style={{ backgroundColor: 'rgb(184, 226, 164)' }}
        className="p-button-rounded"
        tooltip="Учасники"
        onClick={() => openVolunteersDialog(rowData)}
      />
      <Button
        icon="pi pi-list"
        severity="warning"
        style={{ backgroundColor: 'rgb(184, 226, 164)' }}
        className="p-button-rounded"
        tooltip="Завдання"
        onClick={() => openTasksDialog(rowData)}
      />
    </div>
  );

  return (
    <div className="border-round-xl shadow-2 p-4" style={{ backgroundColor: 'white' }}>
      <Toast ref={toast} />
      <h2>Список груп</h2>
      <div className="operation-table">
        <DataTable
          stripedRows
          value={groups}
          paginator
          rows={rows}
          lazy
          first={page * rows}
          totalRecords={totalRecords}
          onPage={onPage}
          loading={loading}
          rowsPerPageOptions={[5, 10, 25, 50]}
          tableStyle={{ minWidth: '70rem' }}
        >
          <Column field="name" header="Назва групи" style={{ width: '25%' }} />
          <Column field="eventName" header="Назва операції" style={{ width: '30%' }} />
          <Column field="leaderName" header="Лідер групи" style={{ width: '25%' }} />
          <Column body={actionTemplate} header="Дії" style={{ width: '2%' }} />
        </DataTable>
      </div>

      {selectedGroup && (
        <>
          <GroupVolunteersDialog
            visible={volunteersDialogVisible}
            group={selectedGroup}
            onHide={() => setVolunteersDialogVisible(false)}
          />

          <GroupTasksDialog
            visible={tasksDialogVisible}
            group={selectedGroup}
            onHide={() => setTasksDialogVisible(false)}
          />
        </>
      )}
    </div>
  );
}