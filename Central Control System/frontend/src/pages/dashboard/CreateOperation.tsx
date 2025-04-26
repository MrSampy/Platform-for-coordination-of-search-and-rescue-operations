import React, { useEffect, useRef, useState } from 'react';
import { InputText } from 'primereact/inputtext';
import { Dropdown } from 'primereact/dropdown';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast';
import { Dialog } from 'primereact/dialog';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import LocationPickerMap from '../../components/LocationPickerMap';
import { TokenInfoDTO, UserDTO } from '../../types/authTypes';
import {
  DistrictDTO,
  ResourceDTO,
  ResourceMeasurementUnitDTO,
  MeasurementUnitDTO
} from '../../types/utilsTypes';
import {
  CreateEventDTO,
  EventTypeDTO,
  OperationWorkerDTO,
  CreateResourcesEventDTO
} from '../../types/eventTypes';

export default function CreateOperation() {
  const [formData, setFormData] = useState<CreateEventDTO>({
    name: '',
    longitude: 0,
    latitude: 0,
    eventTypeGID: '',
    districtGID: '',
    coordinatorGID: '',
    userGID: '',
  });

  const [formErrors, setFormErrors] = useState<{ [key in keyof CreateEventDTO]?: boolean }>({});
  const [districts, setDistricts] = useState<DistrictDTO[]>([]);
  const [resources, setResources] = useState<ResourceDTO[]>([]);
  const [measures, setMeasures] = useState<MeasurementUnitDTO[]>([]);
  const [eventTypes, setEventTypes] = useState<EventTypeDTO[]>([]);
  const [workers, setWorkers] = useState<OperationWorkerDTO[]>([]);
  const [resourcesEvent, setResourcesEvent] = useState<CreateResourcesEventDTO[]>([]);

  const [dialogVisible, setDialogVisible] = useState(false);
  const [selectedResource, setSelectedResource] = useState<string>('');
  const [selectedUnit, setSelectedUnit] = useState<string>('');
  const [requiredQuantity, setRequiredQuantity] = useState<number>(0);
  const [filteredUnits, setFilteredUnits] = useState<MeasurementUnitDTO[]>([]);

  const [loading, setLoading] = useState(false);
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchInitialData = async () => {
      const tokenStr = localStorage.getItem('token');
      const userStr = localStorage.getItem('user');
      if (!tokenStr || !userStr) return;

      const tokenInfo = JSON.parse(tokenStr) as TokenInfoDTO;
      const user = JSON.parse(userStr) as UserDTO;
      setFormData(prev => ({ ...prev, userGID: user.id }));

      const headers = { Authorization: `Bearer ${tokenInfo.token}` };

      try {
        const [districtRes, eventTypeRes, workerRes, resourcesRes, measuresRes] = await Promise.all([
          axios.get<DistrictDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/district?pageNumber=0&pageSize=0`, { headers }),
          axios.get<EventTypeDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/eventType?pageNumber=0&pageSize=0`, { headers }),
          axios.get<OperationWorkerDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/operationworker/byRole/Coordinator`, { headers }),
          axios.get<ResourceDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/resource?pageNumber=0&pageSize=0`, { headers }),
          axios.get<MeasurementUnitDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/measurementUnit?pageNumber=0&pageSize=0`, { headers }),
        ]);

        setDistricts(districtRes.data);
        setEventTypes(eventTypeRes.data);
        setWorkers(workerRes.data);
        setResources(resourcesRes.data);
        setMeasures(measuresRes.data);
      } catch (err) {
        console.error('Failed to fetch data', err);
      }
    };

    fetchInitialData();
  }, []);

  const handleChange = (field: keyof CreateEventDTO, value: any) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    setFormErrors(prev => ({ ...prev, [field]: false }));
  };

  const handleSubmit = async () => {
    const errors: { [key in keyof CreateEventDTO]?: boolean } = {};
  
    (['name', 'eventTypeGID', 'districtGID', 'coordinatorGID', 'latitude', 'longitude'] as (keyof CreateEventDTO)[]).forEach(key => {
      const value = formData[key];
      if ((typeof value === 'string' && value.trim() === '') || (typeof value === 'number' && value === 0)) {
        errors[key] = true;
      }
    });
  
    if (Object.keys(errors).length > 0) {
      setFormErrors(errors);
      toast.current?.show({ severity: 'error', summary: 'Помилка', detail: 'Заповніть всі обовʼязкові поля.', life: 3000 });
      return;
    }
  
    const tokenStr = localStorage.getItem('token');
    if (!tokenStr) return;
    const tokenInfo = JSON.parse(tokenStr) as TokenInfoDTO;
    const headers = { Authorization: `Bearer ${tokenInfo.token}` };
  
    try {
      setLoading(true);
  
      // 1. Створюємо операцію
      const eventResponse = await axios.post(`${process.env.REACT_APP_API_BASE_URL}/event/create-with-usergid`, formData, { headers });
      const createdEventGID = eventResponse.data?.gid;
  
      if (!createdEventGID) {
        throw new Error('Помилка отримання GID операції');
      }
  
      // 2. Для кожного ресурсу ставимо eventGID і створюємо
      for (const res of resourcesEvent) {
        await axios.post(`${process.env.REACT_APP_API_BASE_URL}/resourcesEvent`, {
          ...res,
          eventGID: createdEventGID,
        }, { headers });
      }
  
      toast.current?.show({ severity: 'success', summary: 'Операцію створено', detail: '', life: 3000 });
      setTimeout(() => navigate('/dashboard/operations'), 1000);
    } catch (err: any) {
      console.error(err);
      toast.current?.show({ severity: 'error', summary: 'Помилка', detail: 'Не вдалося створити операцію або ресурси', life: 4000 });
    } finally {
      setLoading(false);
    }
  };
  
  const handleResourceChange = async (resourceGID: string) => {
    setSelectedResource(resourceGID);
    setSelectedUnit(''); // clear selected unit
  
    if (!resourceGID) {
      setFilteredUnits([]);
      return;
    }
  
    try {
      const tokenStr = localStorage.getItem('token');
      if (!tokenStr) return;
      const tokenInfo = JSON.parse(tokenStr) as TokenInfoDTO;
      const headers = { Authorization: `Bearer ${tokenInfo.token}` };
      const res = await axios.get<ResourceMeasurementUnitDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/resourceMeasurementUnit/by-resource/${resourceGID}`, {headers: headers});
      const unitGIDs = res.data.map(r => r.unitGID);
      setFilteredUnits(measures.filter(m => unitGIDs.includes(m.gid)));
    } catch (e) {
      console.error('Не вдалося завантажити одиниці виміру');
    }
  };

  const openResourceDialog = async () => {
    setSelectedResource('');
    setSelectedUnit('');
    setRequiredQuantity(0);
    setFilteredUnits([]);
    setDialogVisible(true);
  };

  const addResourceEvent = () => {
    if (!selectedResource || !selectedUnit || requiredQuantity <= 0) return;
    setResourcesEvent(prev => [...prev, {
      resourceGID: selectedResource,
      eventGID: '',
      requiredQuantity,
      availableQuantity: 0
    }]);
    setDialogVisible(false);
    setSelectedResource('');
    setSelectedUnit('');
    setRequiredQuantity(0);
  };

  return (
    <div className='grid justify-content-center'>
      <Toast ref={toast} />
      <div className="col-12 md:col-10 lg:col-8">
        <div className="border-round-xl shadow-2 p-4" style={{ backgroundColor: 'white' }}>
          <h2>Створення операції</h2>

          <div className="p-fluid formgrid grid">
            <div className="field col-12 md:col-6">
              <label htmlFor="name">Назва</label>
              <InputText id="name" className={formErrors.name ? 'p-invalid' : ''} value={formData.name} onChange={(e) => handleChange('name', e.target.value)} />
            </div>

            <div className="field col-12 md:col-6">
              <label htmlFor="district">Район</label>
              <Dropdown
                id="district"
                className={formErrors.districtGID ? 'p-invalid' : ''}
                value={formData.districtGID}
                onChange={(e) => handleChange('districtGID', e.value)}
                options={districts}
                optionLabel="name"
                optionValue="gid"
                placeholder="Оберіть район"
              />
            </div>

            <div className="field col-12 md:col-6">
              <label htmlFor="eventType">Тип події</label>
              <Dropdown
                id="eventType"
                className={formErrors.eventTypeGID ? 'p-invalid' : ''}
                value={formData.eventTypeGID}
                onChange={(e) => handleChange('eventTypeGID', e.value)}
                options={eventTypes}
                optionLabel="name"
                optionValue="gid"
                placeholder="Оберіть тип події"
              />
            </div>

            <div className="field col-12 md:col-6">
              <label htmlFor="worker">Координатор</label>
              <Dropdown
                id="worker"
                className={formErrors.coordinatorGID ? 'p-invalid' : ''}
                placeholder="Оберіть координатора"
                value={formData.coordinatorGID}
                onChange={(e) => handleChange('coordinatorGID', e.value)}
                options={workers}
                itemTemplate={(option) => `${option.surname} ${option.name} ${option.secondName}`}
                valueTemplate={(option) => option ? `${option.surname} ${option.name} ${option.secondName}` : 'Оберіть координатора'}
                optionValue="gid"
              />
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
              <label htmlFor="latitude">Широта</label>
              <InputText id="latitude" className={formErrors.latitude ? 'p-invalid' : ''} value={formData.latitude.toFixed(6)} disabled />
            </div>

            <div className="field col-6">
              <label htmlFor="longitude">Довгота</label>
              <InputText id="longitude" className={formErrors.longitude ? 'p-invalid' : ''} value={formData.longitude.toFixed(6)} disabled />
            </div>

            <div className="field col-12 flex justify-content-end">
              <Button icon="pi pi-plus" style={{backgroundColor: "rgb(163, 192, 166)"}} label="Додати ресурс" onClick={openResourceDialog} className="mr-2" />
              <Button icon="pi pi-check" label="Створити" onClick={handleSubmit} loading={loading} />
            </div>

            <div className="col-12">
              <h4>Ресурси</h4>
              <ul>
                {resourcesEvent.map((r, i) => (
                  <li key={i}>{`Ресурс: ${resources.find((element) => element.gid === r.resourceGID)?.name}, К-сть: ${r.requiredQuantity}`}</li>
                ))}
              </ul>
            </div>
            
          </div>
        </div>
      </div>
      <Dialog header="Новий ресурс" visible={dialogVisible} style={{ width: '30vw' }} onHide={() => setDialogVisible(false)}>
        <div className="p-fluid">
          <div className="field">
            <label>Ресурс</label>
            <Dropdown value={selectedResource} onChange={(e) => handleResourceChange(e.value)} options={resources} optionLabel="name" optionValue="gid" placeholder="Оберіть ресурс" />
            </div>
          <div className="field">
            <label>Одиниця</label>
            <Dropdown value={selectedUnit} onChange={(e) => setSelectedUnit(e.value)} options={filteredUnits} optionLabel="name" optionValue="gid" placeholder="Оберіть одиницю" />
          </div>
          <div className="field">
            <label>Необхідна кількість</label>
            <InputText value={requiredQuantity.toFixed(6)} onChange={(e) => setRequiredQuantity(Number(e.target.value))} type="number" />
          </div>
          <div className="flex justify-content-end gap-2">
            <Button label="Зберегти" icon="pi pi-check" onClick={addResourceEvent} />
            <Button label="Скасувати" icon="pi pi-times" onClick={() => setDialogVisible(false)} className="p-button-secondary" />
          </div>
        </div>
      </Dialog>
    </div>
  );
}