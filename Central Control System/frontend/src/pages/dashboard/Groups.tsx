import { useState, useEffect, useRef, useMemo } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import axios from 'axios';
import { TokenInfoDTO, UserDTO } from '../../types/authTypes';
import { DetailGroup, GroupPaginationQuery } from '../../types/groupTypes'; 
import { OperationWorkerDTO } from '../../types/eventTypes';
import { Toast } from 'primereact/toast';
import { ErrorModel } from '../../types/commonTypes';

export default function GroupsPage() {
  const [groups, setGroups] = useState<DetailGroup[]>([]);
  const [totalRecords, setTotalRecords] = useState(0);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [rows, setRows] = useState(5);
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
    fetchGroups(page, rows);
  }, [page, rows]);

  async function fetchGroups(page: number, rows: number) {
    try {
      setLoading(true);
      const tokenInfo = localStorage.getItem('token');
      if (tokenInfo !== null) {
        const token = JSON.parse(tokenInfo) as TokenInfoDTO;
        const paginationQuery: GroupPaginationQuery = {
          pageNumber: page + 1,
          pageSize: rows,
          leaderGID: null,
          eventGID: null
        };
        const response = await axios.post<{ items: DetailGroup[], totalCount: number }>(
          `${process.env.REACT_APP_API_BASE_URL}/group/sort`,
          paginationQuery,
          {
            headers: {
              Authorization: `Bearer ${token.token}`,
            },
          }
        );
        setGroups(response.data.items);
        setTotalRecords(response.data.totalCount);
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
  }

  function onPage(event: any) {
    setPage(event.page);
    setRows(event.rows);
  }

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
          tableStyle={{ minWidth: '60rem' }}
        >
          <Column field="name" header="Назва групи" style={{ width: '30%' }} />
          <Column field="eventName" header="Назва операції" style={{ width: '35%' }} />
          <Column field="leaderName" header="Лідер групи" style={{ width: '35%' }} />
        </DataTable>
      </div>
    </div>
  );
}
