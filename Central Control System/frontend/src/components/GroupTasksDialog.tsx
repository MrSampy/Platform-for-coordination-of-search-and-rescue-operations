import { useState, useEffect, useRef } from 'react';
import { Dialog } from 'primereact/dialog';
import { Button } from 'primereact/button';
import { InputText } from 'primereact/inputtext';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Dropdown } from 'primereact/dropdown';
import { Toast } from 'primereact/toast';
import { DetailGroup, CreateOperationTaskDTO, OperationTaskStatusDTO } from '../types/groupTypes';
import { TokenInfoDTO } from '../types/authTypes';
import axios from 'axios';
import { getValidToken } from '../services/commonService';

interface GroupTasksDialogProps {
  visible: boolean;
  group: DetailGroup;
  onHide: () => void;
}

export default function GroupTasksDialog({ visible, group, onHide }: GroupTasksDialogProps) {
  const toast = useRef<Toast>(null);
  const [tasks, setTasks] = useState<(CreateOperationTaskDTO & { deleted?: boolean; edited?: boolean })[]>([]);
  const [taskStatuses, setTaskStatuses] = useState<OperationTaskStatusDTO[]>([]);
  const [taskDialogVisible, setTaskDialogVisible] = useState(false);
  const [editingTask, setEditingTask] = useState<CreateOperationTaskDTO | null>(null);
  const [taskName, setTaskName] = useState('');
  const [taskDescription, setTaskDescription] = useState('');
  const [taskStatusGID, setTaskStatusGID] = useState('');

  useEffect(() => {
    const loadData = async () => {
      const tokenStr = getValidToken();
      if (!tokenStr || !group) return;

      const token = JSON.parse(tokenStr) as TokenInfoDTO;
      const headers = { Authorization: `Bearer ${token.token}` };

      try {
        const [tasksRes, statusesRes] = await Promise.all([
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/operationTask/byGroupGID/${group.gid}`, { headers }),
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/operationTaskStatus?pageNumber=0&pageSize=0`, { headers })
        ]);

        setTasks(tasksRes.data.map((t: CreateOperationTaskDTO) => ({ ...t })));
        setTaskStatuses(statusesRes.data);
      } catch (error) {
        console.error('Failed to load tasks or statuses', error);
      }
    };

    if (visible) {
      loadData();
    }
  }, [visible, group]);

  const openNewTaskDialog = () => {
    setEditingTask(null);
    setTaskName('');
    setTaskDescription('');
    setTaskStatusGID('');
    setTaskDialogVisible(true);
  };

  const openEditTaskDialog = (task: CreateOperationTaskDTO) => {
    setEditingTask(task);
    setTaskName(task.name);
    setTaskDescription(task.taskDescription);
    setTaskStatusGID(task.taskStatusGID);
    setTaskDialogVisible(true);
  };

  const saveTask = () => {
    if (!taskName.trim() || !taskStatusGID) {
      toast.current?.show({ severity: 'error', summary: 'Помилка', detail: 'Назва і статус обовʼязкові', life: 3000 });
      return;
    }

    if (editingTask) {
      setTasks(prev =>
        prev.map(t => (t === editingTask ? { ...t, name: taskName.trim(), taskDescription: taskDescription.trim(), taskStatusGID, edited: true } : t))
      );
    } else {
      const newTask: CreateOperationTaskDTO = {
        gid: null,
        name: taskName.trim(),
        taskDescription: taskDescription.trim(),
        groupGID: group.gid,
        taskStatusGID: taskStatusGID
      };
      setTasks(prev => [...prev, { ...newTask }]);
    }

    setTaskDialogVisible(false);
  };

  const deleteTask = (task: CreateOperationTaskDTO) => {
    if (!task.gid) {
      setTasks(prev => prev.filter(t => t !== task));
    } else {
      setTasks(prev => prev.map(t => t === task ? { ...t, deleted: true } : t));
    }
  };

  const handleSaveTasks = async () => {
    const tokenStr = getValidToken();
    if (!tokenStr) return;
    const token = JSON.parse(tokenStr) as TokenInfoDTO;
    const headers = { Authorization: `Bearer ${token.token}` };

    try {
      const newTasks = tasks.filter(t => !t.gid && !t.deleted);
      const updatedTasks = tasks.filter(t => t.gid && t.edited && !t.deleted);
      const deletedTasks = tasks.filter(t => t.gid && t.deleted);

      await Promise.all([
        ...newTasks.map(t => axios.post(`${process.env.REACT_APP_API_BASE_URL}/operationTask`, t, { headers })),
        ...updatedTasks.map(t => axios.put(`${process.env.REACT_APP_API_BASE_URL}/operationTask`, t, { headers })),
        ...deletedTasks.map(t => axios.delete(`${process.env.REACT_APP_API_BASE_URL}/operationTask/${t.gid}`, { headers }))
      ]);

      toast.current?.show({ severity: 'success', summary: 'Успіх', detail: 'Завдання збережено', life: 3000 });

      onHide();
    } catch (error) {
      console.error('Failed to save tasks', error);
      toast.current?.show({ severity: 'error', summary: 'Помилка', detail: 'Не вдалося зберегти завдання', life: 3000 });
    }
  };

  return (
    <>
      <Toast ref={toast} />

      <Dialog
        header={`Завдання групи: ${group.name}`}
        visible={visible}
        style={{ width: '60vw' }}
        modal
        onHide={onHide}
        footer={
          <div className="flex justify-content-end gap-2">
            <Button label="Зберегти" icon="pi pi-check" onClick={handleSaveTasks} style={{ backgroundColor: "rgb(85, 147, 240)" }} />
            <Button label="Скасувати" icon="pi pi-times" className="p-button-secondary" onClick={onHide} style={{ backgroundColor: "rgb(192, 48, 48)" }} />
          </div>
        }
      >
        <div className="flex justify-content-end mb-3">
          <Button label="Додати завдання" icon="pi pi-plus" onClick={openNewTaskDialog} style={{ backgroundColor: "rgb(85, 147, 240)" }} />
        </div>

        <DataTable value={tasks.filter(t => !t.deleted)} tableStyle={{ minWidth: '100%' }}>
          <Column field="name" header="Назва завдання" />
          <Column field="taskDescription" header="Опис" />
          <Column field="taskStatusGID" header="Статус" body={(rowData) => taskStatuses.find(s => s.gid === rowData.taskStatusGID)?.name} />
          <Column
            header="Дії"
            body={(rowData: CreateOperationTaskDTO) => (
              <div className="flex gap-2">
                <Button icon="pi pi-pencil" className="p-button-rounded" style={{ backgroundColor: "rgb(85, 147, 240)" }} onClick={() => openEditTaskDialog(rowData)} />
                <Button icon="pi pi-trash" className="p-button-rounded p-button-danger" style={{ backgroundColor: "rgb(192, 48, 48)" }} onClick={() => deleteTask(rowData)} />
              </div>
            )}
            style={{ width: '8rem' }}
          />
        </DataTable>
      </Dialog>

      <Dialog
        header={editingTask ? 'Редагувати завдання' : 'Нове завдання'}
        visible={taskDialogVisible}
        style={{ width: '30vw' }}
        modal
        onHide={() => setTaskDialogVisible(false)}
        footer={
          <div className="flex justify-content-end gap-2">
            <Button label="Зберегти" icon="pi pi-check" onClick={saveTask} style={{ backgroundColor: "rgb(85, 147, 240)" }} />
            <Button label="Скасувати" icon="pi pi-times" className="p-button-secondary" onClick={() => setTaskDialogVisible(false)} style={{ backgroundColor: "rgb(192, 48, 48)" }} />
          </div>
        }
      >
        <div className="p-fluid">
          <div className="field">
            <label>Назва завдання</label>
            <InputText value={taskName} onChange={(e) => setTaskName(e.target.value)} maxLength={100} autoFocus className="w-full" />
          </div>
          <div className="field">
            <label>Опис завдання</label>
            <InputText value={taskDescription} onChange={(e) => setTaskDescription(e.target.value)} maxLength={255} className="w-full" />
          </div>
          <div className="field">
            <label>Статус завдання</label>
            <Dropdown
              value={taskStatusGID}
              options={taskStatuses}
              optionLabel="name"
              optionValue="gid"
              onChange={(e) => setTaskStatusGID(e.value)}
              placeholder="Оберіть статус"
              className="w-full"
            />
          </div>
        </div>
      </Dialog>
    </>
  );
}
