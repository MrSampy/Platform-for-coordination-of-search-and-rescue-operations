import { useState, useEffect, useRef } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import axios from 'axios';
import { TokenInfoDTO } from '../../types/authTypes';
import { DetailEvent, EventPaginationQuery } from '../../types/eventTypes'; 
import { EventStatusComplete } from '../../types/constants'
import { Button } from 'primereact/button';
import { Toast } from "primereact/toast";
import { ErrorModel } from "../../types/commonTypes";
import { getValidToken } from '../../services/commonService';

export default function Reports() {
    const [events, setEvents] = useState<DetailEvent[]>([]);
    const [totalRecords, setTotalRecords] = useState(0);
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(0);
    const [rows, setRows] = useState(5);
    const toast = useRef<Toast>(null);

    useEffect(() => {
      fetchEvents(page, rows);
    }, [page, rows]);
  
    async function fetchEvents(page: number, rows: number) {
      try {
        setLoading(true);
        const tokenInfo = getValidToken();
        if(tokenInfo !== null && tokenInfo !== undefined){
          const token = JSON.parse(tokenInfo) as TokenInfoDTO;
          const paginationQuery: EventPaginationQuery = {
            pageNumber: page + 1,
            pageSize: rows,
            eventStatusGID: EventStatusComplete.gid,
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
    function onPage(event: any) {
      setPage(event.page);
      setRows(event.rows);
    }
    
    async function getReport(event: DetailEvent) {
        try {
            const tokenInfo = getValidToken();
            if (tokenInfo !== null && tokenInfo !== undefined) {
                const token = JSON.parse(tokenInfo) as TokenInfoDTO;
                const response = await axios.get(
                    `${process.env.REACT_APP_API_BASE_URL}/event/report/${event.gid}`,
                    {
                        headers: {
                            Authorization: `Bearer ${token.token}`,
                        },
                        responseType: 'blob' 
                    }
                );
    
                const url = window.URL.createObjectURL(new Blob([response.data]));
    
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', `Report_${event.name || 'Report'}.pdf`);
                document.body.appendChild(link);
                link.click();
    
                // Clean up
                link.parentNode?.removeChild(link);
                window.URL.revokeObjectURL(url);
            }
        } catch (error: any) {
            const apiError = error.response?.data as ErrorModel;
            toast.current?.show({
                severity: "error",
                summary: "Failed to download report",
                detail: apiError?.message || "Unknown error",
                life: 3000
            });
        }
    }
    

    return (        
    <div className="border-round-xl shadow-2 p-4" style={{ backgroundColor: 'white' }}>
        <Toast ref={toast} />
        <h2> Звітність</h2>
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
            <Column field="dispatcher" header="Диспетчер" style={{ width: '20%' }} />
            <Column field="coordinator" header="Координатор" style={{ width: '20%' }} />
            <Column
                header="Звіт"
                body={(rowData: DetailEvent) => (
                  <div className="flex gap-2 justify-content-center">
                    <Button className='p-button-rounded' icon="pi pi-file" raised severity="warning" style={{ fontSize: '12px', backgroundColor:'rgb(164, 219, 226)' }} onClick={() => getReport(rowData)}/>
                  </div>
                )}
                style={{ width: '2%' }}
            />
            </DataTable>
        </div>
      </div>
    );
}
