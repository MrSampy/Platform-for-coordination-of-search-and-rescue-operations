import React, { useEffect, useRef, useState } from 'react';
import { InputText } from 'primereact/inputtext';
import { Dropdown } from 'primereact/dropdown';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { Toast } from 'primereact/toast';
import axios from 'axios';
import LocationPickerMap from '../components/LocationPickerMap';
import { TokenInfoDTO } from '../types/authTypes';
import { DistrictDTO } from '../types/utilsTypes';
import { DetailEvent, EventTypeDTO, OperationWorkerDTO, UpdateEventDTO } from '../types/eventTypes';
import {
  EventStatusCreated,
  EventStatusApproved,
  EventStatusRejected,
  EventStatusActive,
  EventStatusComplete,
  EventStatusDeleted
} from '../types/constants';

export default function EditOperationDialog({ selectedEvent, visible, onHide, onUpdate }: {
  selectedEvent: DetailEvent | null,
  visible: boolean,
  onHide: () => void,
  onUpdate: (data: UpdateEventDTO) => void
}) {
  const toast = useRef<Toast>(null);
  const [formData, setFormData] = useState<UpdateEventDTO | null>(null);
  const [eventTypes, setEventTypes] = useState<EventTypeDTO[]>([]);
  const [districts, setDistricts] = useState<DistrictDTO[]>([]);
  const [workers, setWorkers] = useState<OperationWorkerDTO[]>([]);

  const getAvailableStatuses = (currentStatus: string) => {
    switch (currentStatus.toLowerCase()) {
      case EventStatusCreated.gid.toLowerCase():
        return [EventStatusCreated, EventStatusDeleted,];
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
      const tokenStr = localStorage.getItem('token');
      if (!tokenStr || !selectedEvent) return;

      const token = JSON.parse(tokenStr) as TokenInfoDTO;
      const headers = { Authorization: `Bearer ${token.token}` };

      try {
        const [eventTypeRes, districtRes, workersRes] = await Promise.all([
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/eventType?pageNumber=0&pageSize=0`, { headers }),
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/district?pageNumber=0&pageSize=0`, { headers }),
          axios.get(`${process.env.REACT_APP_API_BASE_URL}/operationworker/byRole/Coordinator`, { headers }),
        ]);

        setEventTypes(eventTypeRes.data);
        setDistricts(districtRes.data);
        setWorkers(workersRes.data);

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

  const handleSave = () => {
    if (formData) {
      onUpdate(formData);
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
          <Button label="Зберегти" icon="pi pi-check" onClick={handleSave} />
          <Button label="Відмінити" icon="pi pi-times" onClick={onHide} className="p-button-secondary" />
        </div>
      }
    >
      <Toast ref={toast} />
      {formData && (
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
        </div>
      )}
    </Dialog>
  );
}