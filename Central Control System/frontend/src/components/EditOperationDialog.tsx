import React, { useEffect, useRef, useState } from 'react';
import { InputText } from 'primereact/inputtext';
import { Dropdown } from 'primereact/dropdown';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { Toast } from 'primereact/toast';
import axios from 'axios';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import LocationPickerMap from '../components/LocationPickerMap';
import { TokenInfoDTO } from '../types/authTypes';
import { DistrictDTO, ResourceDTO } from '../types/utilsTypes';
import { DetailEvent, EventTypeDTO, OperationWorkerDTO, UpdateEventDTO, ResourcesEventDTO} from '../types/eventTypes';
import { CreateGroupDTO} from '../types/groupTypes';
import {
  EventStatusCreated,
  EventStatusApproved,
  EventStatusRejected,
  EventStatusActive,
  EventStatusComplete,
  EventStatusDeleted
} from '../types/constants';
import { getValidToken } from '../services/commonService';
import { InputTextarea } from 'primereact/inputtextarea';
export default function EditOperationDialog({ selectedEvent, visible, onHide }: {
  selectedEvent: DetailEvent | null,
  visible: boolean,
  onHide: () => void
}) {
  const toast = useRef<Toast>(null);
  const [formData, setFormData] = useState<UpdateEventDTO | null>(null);
  const [eventTypes, setEventTypes] = useState<EventTypeDTO[]>([]);
  const [districts, setDistricts] = useState<DistrictDTO[]>([]);
  const [workers, setWorkers] = useState<OperationWorkerDTO[]>([]);
  const [groups, setGroups] = useState<CreateGroupDTO[]>([]);
  const [groupDialogVisible, setGroupDialogVisible] = useState(false);
  const [newGroupName, setNewGroupName] = useState<string>('');
  const [editingGroup, setEditingGroup] = useState<CreateGroupDTO | null>(null);
  const [resourcesEvent, setResourcesEvent] = useState<ResourcesEventDTO[]>([]);
  const [resources, setResources] = useState<ResourceDTO[]>([]);
  const [measurementUnits, setMeasurementUnits] = useState<ResourceDTO[]>([]);

  const getAvailableStatuses = (currentStatus: string) => {
    switch (currentStatus.toLowerCase()) {
      case EventStatusCreated.gid.toLowerCase():
        return [EventStatusCreated, EventStatusDeleted];
      case EventStatusApproved.gid.toLowerCase():
        return [EventStatusApproved, EventStatusActive, EventStatusDeleted];
      case EventStatusRejected.gid.toLowerCase():
        return [EventStatusRejected, EventStatusCreated, EventStatusDeleted];
      case EventStatusActive.gid.toLowerCase():
        return [EventStatusActive, EventStatusComplete, EventStatusDeleted];
      case EventStatusComplete.gid.toLowerCase():
        return [EventStatusComplete, EventStatusDeleted];
      case EventStatusDeleted.gid.toLowerCase():
      default:
        return [EventStatusDeleted];
    }
  };

  useEffect(() => {
    const loadFormData = async () => {
      const tokenStr = getValidToken();
      if (!tokenStr || !selectedEvent) return;

      const token = JSON.parse(tokenStr) as TokenInfoDTO;
      const headers = { Authorization: `Bearer ${token.token}` };

      try {
        const [eventTypeRes, districtRes, workersRes, groupRes, resourceRes, measurementUnitRes, resourcesEventRes] = await Promise.all([
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/eventType?pageNumber=0&pageSize=0`, { headers }),
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/district?pageNumber=0&pageSize=0`, { headers }),
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/operationworker/byRole/Coordinator`, { headers }),
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/group/byEventGID/${selectedEvent.gid}`, { headers }),
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/resource?pageNumber=0&pageSize=0`, { headers }),
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/measurementUnit?pageNumber=0&pageSize=0`, { headers }),
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/resourcesEvent/by-event/${selectedEvent.gid}`, { headers }),
        ]);

        setEventTypes(eventTypeRes.data);
        setDistricts(districtRes.data);
        setWorkers(workersRes.data);
        setResources(resourceRes.data);
        setMeasurementUnits(measurementUnitRes.data);
        setResourcesEvent(resourcesEventRes.data);

        const mappedGroups = groupRes.data.map((g: any) => ({
          gid: g.gid,
          name: g.name,
          eventGID: g.eventGID,
          leaderGID: g.leaderGID ?? null
        })) as CreateGroupDTO[];

        setGroups(mappedGroups);

        setFormData({
          gid: selectedEvent.gid,
          name: selectedEvent.name,
          longitude: selectedEvent.longitude,
          latitude: selectedEvent.latitude,
          eventTypeGID: selectedEvent.eventTypeGID,
          districtGID: selectedEvent.districtGID,
          coordinatorGID: selectedEvent.coordinatorGID,
          dispatcherGID: selectedEvent.dispatcherGID,
          eventStatusGID: selectedEvent.eventStatusGID,
          note: selectedEvent.note
        });
      } catch (error) {
        console.error('Failed to load form data', error);
      }
    };

    if (visible) {
      loadFormData();
    }
  }, [selectedEvent, visible]);

  const handleChange = (field: keyof UpdateEventDTO, value: any) => {
    if (formData) {
      setFormData(prev => ({ ...prev!, [field]: value }));
    }
  };

  const handleSave = async () => {
    if (!formData) return;

    const tokenStr = getValidToken();
    if (!tokenStr) return;

    const token = JSON.parse(tokenStr) as TokenInfoDTO;
    const headers = { Authorization: `Bearer ${token.token}` };

    try {
      await axios.put(`${process.env.REACT_APP_API_BASE_URL}/event`, formData, { headers });

      const newGroups = groups.filter(g => !g.gid);
      const updatedGroups = groups.filter(g => g.gid && g.edited);
      const deletedGroups = groups.filter(g => g.gid && g.deleted);

      await Promise.all([
        ...newGroups.map(g => axios.post(`${process.env.REACT_APP_API_BASE_URL}/group`, g, { headers })),
        ...updatedGroups.map(g => axios.put(`${process.env.REACT_APP_API_BASE_URL}/group`, g, { headers })),
        ...deletedGroups.map(g => axios.delete(`${process.env.REACT_APP_API_BASE_URL}/group/${g.gid}`, { headers }))
      ]);

      toast.current?.show({
        severity: 'success',
        summary: 'Успіх',
        detail: 'Операцію та групи оновлено',
        life: 3000
      });

      onHide();
    } catch (error) {
      toast.current?.show({
        severity: 'error',
        summary: 'Помилка',
        detail: 'Не вдалося оновити операцію або групи',
        life: 3000
      });
    } finally{
      window.location.reload();
    }
  };

  const handleAddGroup = () => {
    if (!newGroupName.trim() || !formData) return;

    if (editingGroup) {
      setGroups(prev => prev.map(g => g === editingGroup ? { ...g, name: newGroupName, edited: true } : g));
    } else {
      const newGroup: CreateGroupDTO = {
        gid: null,
        name: newGroupName.trim(),
        eventGID: formData.gid,
        leaderGID: null,
        deleted: null,
        edited: null
      };
      setGroups(prev => [...prev, newGroup]);
    }

    setNewGroupName('');
    setEditingGroup(null);
    setGroupDialogVisible(false);
  };

  const handleEditGroup = (group: CreateGroupDTO) => {
    setNewGroupName(group.name);
    setEditingGroup(group);
    setGroupDialogVisible(true);
  };

  const handleDeleteGroup = (group: CreateGroupDTO) => {
    if (!group.gid) {
      setGroups(prev => prev.filter(g => g !== group));
    } else {
      setGroups(prev => prev.map(g => g === group ? { ...g, deleted: true } : g));
    }
  };

  const availableStatuses = formData ? getAvailableStatuses(formData.eventStatusGID) : [];

  return (
    <Dialog
      header="Редагування операції"
      visible={visible}
      style={{ width: '50vw' }}
      onHide={onHide}
      footer={
        <div className="flex justify-content-end gap-2">
          <Button label="Зберегти" icon="pi pi-check" onClick={handleSave} style={{backgroundColor: "rgb(85, 147, 240)"}} />
          <Button label="Відмінити" icon="pi pi-times" onClick={onHide} className="p-button-secondary" style={{ backgroundColor: "rgb(192, 48, 48)" }} />
        </div>
      }
    >
      <Toast ref={toast} />
      {formData && (
        <>
          <div className="p-fluid formgrid grid">
            <div className="field col-12">
              <label>Назва</label>
              <InputText value={formData.name} onChange={(e) => handleChange('name', e.target.value)} />
            </div>
            <div className="field col-6">
              <label>Широта</label>
              <InputText value={formData.latitude.toFixed(6)} disabled />
            </div>
            <div className="field col-6">
              <label>Довгота</label>
              <InputText value={formData.longitude.toFixed(6)} disabled />
            </div>
            <div className="field col-6">
              <label>Тип події</label>
              <Dropdown value={formData.eventTypeGID} options={eventTypes} onChange={(e) => handleChange('eventTypeGID', e.value)} optionLabel="name" optionValue="gid" placeholder="Оберіть тип" />
            </div>
            <div className="field col-6">
              <label>Район</label>
              <Dropdown value={formData.districtGID} options={districts} onChange={(e) => handleChange('districtGID', e.value)} optionLabel="name" optionValue="gid" placeholder="Оберіть район" />
            </div>
            <div className="field col-12">
              <label htmlFor="map">Оберіть місце на мапі</label>
              <LocationPickerMap
                lat={formData.latitude}
                lng={formData.longitude}
                onLocationChange={(lat, lng) => {
                  handleChange('latitude', lat);
                  handleChange('longitude', lng);
                }}
              />
            </div>
            <div className="field col-6">
              <label>Координатор</label>
              <Dropdown
                value={formData.coordinatorGID}
                options={workers}
                onChange={(e) => handleChange('coordinatorGID', e.value)}
                itemTemplate={(option) => `${option.surname} ${option.name} ${option.secondName}`}
                valueTemplate={(option) => option ? `${option.surname} ${option.name} ${option.secondName}` : 'Оберіть координатора'}
                optionValue="gid"
                placeholder="Оберіть координатора"
              />
            </div>
            <div className="field col-6">
              <label>Статус</label>
              <Dropdown
                value={formData.eventStatusGID}
                options={availableStatuses}
                onChange={(e) => handleChange('eventStatusGID', e.value)}
                optionLabel="name"
                optionValue="gid"
                placeholder="Оберіть статус"
              />
            </div>
            <div className="col-12">
            <h4>Коментар</h4>
              <InputTextarea id="note" value={formData.note === null ? '' : formData.note}
               onChange={(e) => handleChange('eventStatusGID', e.target.value)} rows={4} className="w-full" disabled/>
            </div>
            <div className="col-12">
              <h4>Ресурси</h4>
              <ul>
                {resourcesEvent.map((r, i) => (
                  <li key={i}>{`Ресурс: ${resources.find((element) => element.gid === r.resourceGID)?.name},
                   К-сть: ${r.availableQuantity}/${r.requiredQuantity}
                  (${measurementUnits.find((element) => element.gid === r.measurementUnitGID)?.name})
                  `}</li>
                ))}
              </ul>
            </div>
          </div>
          
          <div className="mt-4">
            <div className="flex justify-content-between align-items-center mb-2">
              <h4>Групи</h4>
              <Button icon="pi pi-plus" style={{backgroundColor: "rgb(163, 192, 166)"}} label="Додати групу" onClick={() => { setNewGroupName(''); setEditingGroup(null); setGroupDialogVisible(true); }} />
            </div>
            <DataTable value={groups.filter(g => !g.deleted)} tableStyle={{ minWidth: '100%' }}>
              <Column field="name" header="Назва групи" />
              <Column
                header=""
                body={(rowData: CreateGroupDTO) => (
                  <div className="flex gap-2">
                    <Button icon="pi pi-pencil" className="p-button-rounded" style={{ backgroundColor: "rgb(85, 147, 240)" }} onClick={() => handleEditGroup(rowData)} />
                    <Button icon="pi pi-trash" className="p-button-rounded p-button-danger" onClick={() => handleDeleteGroup(rowData)} style={{ backgroundColor: "rgb(192, 48, 48)" }}/>
                  </div>
                )}
                style={{ width: '7%' }}
              />
            </DataTable>
          </div>

          <Dialog
            header={editingGroup ? 'Редагувати групу' : 'Нова група'}
            visible={groupDialogVisible}
            style={{ width: '30vw' }}
            onHide={() => setGroupDialogVisible(false)}
            footer={
              <div className="flex justify-content-end gap-2">
                <Button label={editingGroup ? 'Зберегти' : 'Додати'} icon="pi pi-check" onClick={handleAddGroup} className="p-button-sm p-button-success" style={{backgroundColor: "rgb(85, 147, 240)"}} />
                <Button label="Скасувати" icon="pi pi-times" onClick={() => setGroupDialogVisible(false)} className="p-button-sm p-button-secondary"  style={{ backgroundColor: "rgb(192, 48, 48)" }}/>
              </div>
            }
          >
            <div className="p-fluid">
              <div className="field">
                <label htmlFor="groupName">Назва групи</label>
                <InputText id="groupName" value={newGroupName} onChange={(e) => setNewGroupName(e.target.value)} maxLength={100} autoFocus className="w-full" />
              </div>
            </div>
          </Dialog>
        </>
      )}
    </Dialog>
  );
}