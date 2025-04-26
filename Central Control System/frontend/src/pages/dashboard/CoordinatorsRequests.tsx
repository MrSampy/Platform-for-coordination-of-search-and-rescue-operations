import { useState, useEffect, useRef } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Dialog } from 'primereact/dialog';
import { Button } from 'primereact/button';
import axios from 'axios';
import { TokenInfoDTO } from '../../types/authTypes';
import { MessageDTO, MessagePaginationQuery, OperationWorkerDTO, DetailEvent, EventPaginationQuery, CreateMessageDTO } from '../../types/eventTypes'; 
import { Toast } from "primereact/toast";
import { ErrorModel } from "../../types/commonTypes";
import { Dropdown } from 'primereact/dropdown';
import { EventStatusActive } from '../../types/constants'

export default function CoordinatorsRequests() {
    const [messages, setMessages] = useState<MessageDTO[]>([]);
    const [totalRecords, setTotalRecords] = useState(0);
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(0);
    const [rows, setRows] = useState(5);
    const toast = useRef<Toast>(null);
    const [addDialogVisible, setAddDialogVisible] = useState(false);
    const [events, setEvents] = useState<DetailEvent[]>([]);
    const [selectedEvent, setSelectedEvent] = useState<DetailEvent | null>(null);
    const [messageText, setMessageText] = useState("");

    const [selectedMessage, setSelectedMessage] = useState<MessageDTO | null>(null);
    const [dialogVisible, setDialogVisible] = useState(false);

    useEffect(() => {
        fetchMessages(page, rows);
        fetchEvents(); 
    }, [page, rows]);

    async function fetchMessages(page: number, rows: number) {
        try {
            setLoading(true);
            const tokenInfo = localStorage.getItem('token');
            const operationWorkerInfo = localStorage.getItem('operationWorker');
            if (tokenInfo !== null && operationWorkerInfo !== null) {
                const token = JSON.parse(tokenInfo) as TokenInfoDTO;
                const operationWorker = JSON.parse(operationWorkerInfo) as OperationWorkerDTO;
                const paginationQuery: MessagePaginationQuery = {
                    pageNumber: page + 1,
                    pageSize: rows,
                    from: null,
                    to: operationWorker.gid,
                    isRead: null,
                    eventGID: null
                };
                const response = await axios.post<{ items: MessageDTO[], totalCount: number }>(
                    `${process.env.REACT_APP_API_BASE_URL}/message/get`,
                    paginationQuery,
                    {
                        headers: {
                            Authorization: `Bearer ${token.token}`,
                        },
                    }
                );
                setMessages(response.data.items);
                setTotalRecords(response.data.totalCount);
            }
        } catch (error: any) {
            const apiError = error.response?.data as ErrorModel;
            toast.current?.show({
                severity: "error",
                summary: "Failed to fetch messages",
                detail: apiError.message,
                life: 3000
            });
        } finally {
            setLoading(false);
        }
    }

    const fetchEvents = async () => {
        const tokenStr = localStorage.getItem('token');
        if (!tokenStr) return;
      
        const token = JSON.parse(tokenStr) as TokenInfoDTO;
      
        try {
            const paginationQuery: EventPaginationQuery = {
                pageNumber: 0,
                pageSize: 0,
                eventStatusGID: EventStatusActive.gid,
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
        } catch (err) {
          console.error('Failed to load events', err);
        }
      };

    function onPage(event: any) {
        setPage(event.page);
        setRows(event.rows);
    }

    const onRowClick = (e: any) => {
        setSelectedMessage(e.data);
        setDialogVisible(true);
    };

    const markAsRead = async () => {
        if (!selectedMessage) return;
        try {
            const tokenInfo = localStorage.getItem('token');
            if (!tokenInfo) return;
            const token = JSON.parse(tokenInfo) as TokenInfoDTO;

            await axios.put(
                `${process.env.REACT_APP_API_BASE_URL}/message/read/${selectedMessage.gid}`,
                {},
                {
                    headers: {
                        Authorization: `Bearer ${token.token}`,
                    },
                }
            );

            toast.current?.show({
                severity: "success",
                summary: "Повідомлення прочитано",
                life: 2000,
            });

            setDialogVisible(false);
            fetchMessages(page, rows);
        } catch (err: any) {
            const apiError = err.response?.data as ErrorModel;
            toast.current?.show({
                severity: "error",
                summary: "Помилка",
                detail: apiError?.message || "Невідома помилка",
                life: 3000
            });
        }
    };
    const submitMessage = async () => {
        const tokenStr = localStorage.getItem('token');
        const operationWorkerInfo = localStorage.getItem('operationWorker');
      
        if (!tokenStr || !operationWorkerInfo || !selectedEvent || messageText.trim() === "") return;
      
        const token = JSON.parse(tokenStr) as TokenInfoDTO;
        const operationWorker = JSON.parse(operationWorkerInfo) as OperationWorkerDTO;
        const createMessage: CreateMessageDTO = {
            from: operationWorker.gid,
            to: selectedEvent.coordinatorGID,
            eventGID: selectedEvent.gid,
            text: messageText,
            isRead: false
        };

        try {
          await axios.post(
            `${process.env.REACT_APP_API_BASE_URL}/message`,
            createMessage,
            {
              headers: {
                Authorization: `Bearer ${token.token}`,
              },
            }
          );
      
          toast.current?.show({
            severity: "success",
            summary: "Повідомлення надіслано",
            life: 2000
          });
      
          setAddDialogVisible(false);
          setSelectedEvent(null);
          setMessageText("");
          fetchMessages(page, rows); // Refresh list
        } catch (err: any) {
          const apiError = err.response?.data as ErrorModel;
          toast.current?.show({
            severity: "error",
            summary: "Помилка",
            detail: apiError?.message || "Невідома помилка",
            life: 3000
          });
        }
      };
      
    return (
        <div className='operation-table'>
            <h2>Непрочитані повідомлення</h2>
            <Toast ref={toast} />

            <DataTable
                stripedRows
                value={messages}
                paginator
                rows={rows}
                lazy
                first={page * rows}
                totalRecords={totalRecords}
                onPage={onPage}
                loading={loading}
                rowsPerPageOptions={[5, 10, 25, 50]}
                tableStyle={{ minWidth: '90rem' }}
                onRowClick={onRowClick}
                rowClassName={() => 'clickable-row'} 
            >
                <Column
                    body={() => <i className="pi pi-comments" style={{ fontSize: '1rem', color: '#888' }} />}
                    style={{ width: '3%', textAlign: 'center' }}
                    header=""
                    />
                <Column field="sender" header="Відправник" style={{ width: '30%' }} />
                <Column field="receiver" header="Отримувач" style={{ width: '30%' }} />
                <Column field="eventName" header="Подія" style={{ width: '30%' }} />
                <Column
                    header="Статус"
                    body={(rowData: MessageDTO) => (
                        <span style={{ color: rowData.isRead ? 'green' : 'red', fontWeight: 'bold' }}>
                        {rowData.isRead ? 'Прочитано' : 'Не прочитано'}
                        </span>
                    )}
                    style={{ width: '15%' }}
                />
            </DataTable>
            <Button
                label="Додати"
                icon="pi pi-plus"
                className="mt-3"
                onClick={() => setAddDialogVisible(true)}
                />

            <Dialog
            header="Нове повідомлення"
            visible={addDialogVisible}
            style={{ width: '40vw', maxWidth: '600px' }}
            className="p-fluid"
            onHide={() => setAddDialogVisible(false)}
            footer={
                <div className="flex justify-content-end gap-2">
                <Button
                    label="ВІДПРАВИТИ"
                    icon="pi pi-send"
                    className="p-button-primary"
                    disabled={!selectedEvent || messageText.trim() === ""}
                    onClick={submitMessage}
                />
                <Button
                    label="ВІДМІНИТИ"
                    icon="pi pi-times"
                    className="p-button-secondary"
                    onClick={() => setAddDialogVisible(false)}
                />
                </div>
            }
            >
            <div className="formgrid grid">
                <div className="field col-12">
                <label htmlFor="event" className="font-semibold">Активна подія</label>
                <Dropdown
                    id="event"
                    value={selectedEvent}
                    onChange={(e) => setSelectedEvent(e.value)}
                    options={events}
                    optionLabel="name"
                    placeholder="Оберіть подію"
                    className="w-full"
                />
                </div>

                <div className="field col-12">
                <label htmlFor="text" className="font-semibold">Текст</label>
                <textarea
                    id="text"
                    rows={5}
                    maxLength={500}
                    value={messageText}
                    onChange={(e) => setMessageText(e.target.value)}
                    placeholder="Введіть повідомлення (макс. 500 символів)"
                    className="p-inputtext p-component w-full"
                />
                <div className="text-right text-sm mt-1 text-color-secondary">
                    {messageText.length}/500
                </div>
                </div>
            </div>
            </Dialog>

            <Dialog
                header="Повідомлення"
                visible={dialogVisible}
                style={{ width: '50vw' }}
                onHide={() => setDialogVisible(false)}
                footer={
                    <div className="flex justify-content-end gap-2">                        
                        <Button label="Прочитати" icon="pi pi-check" onClick={markAsRead} />
                        <Button label="Відмінити" icon="pi pi-times" onClick={() => setDialogVisible(false)} autoFocus />
                    </div>
                }
            >
                <p>{selectedMessage?.text}</p>
            </Dialog>
        </div>
    );
}
