import React, { useEffect, useRef, useState } from 'react';
import { InputText } from 'primereact/inputtext';
import { Dropdown } from 'primereact/dropdown';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import LocationPickerMap from '../../components/LocationPickerMap';
import { TokenInfoDTO, UserDTO } from '../../types/authTypes';
import { DistrictDTO } from '../../types/utilsTypes';
import { CreateEventDTO, EventTypeDTO, OperationWorkerDTO } from '../../types/eventTypes';

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
  const [eventTypes, setEventTypes] = useState<EventTypeDTO[]>([]);
  const [workers, setWorkers] = useState<OperationWorkerDTO[]>([]);
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
        const [districtRes, eventTypeRes, workerRes] = await Promise.all([
          axios.get<DistrictDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/district?pageNumber=0&pageSize=0`, { headers }),
          axios.get<EventTypeDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/eventType?pageNumber=0&pageSize=0`, { headers }),
          axios.get<OperationWorkerDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/operationworker/byRole/Coordinator`, { headers }),
        ]);

        setDistricts(districtRes.data);
        setEventTypes(eventTypeRes.data);
        setWorkers(workerRes.data);
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
      if (
        (typeof value === 'string' && value.trim() === '') ||
        (typeof value === 'number' && value === 0)
      ) {
        errors[key] = true;
      }
    });

    if (Object.keys(errors).length > 0) {
      setFormErrors(errors);
      toast.current?.show({
        severity: 'error',
        summary: 'Помилка',
        detail: 'Будь ласка, заповніть всі обовʼязкові поля.',
        life: 3000
      });
      return;
    }

    const tokenStr = localStorage.getItem('token');
    if (!tokenStr) return;

    const tokenInfo = JSON.parse(tokenStr) as TokenInfoDTO;
    const headers = { Authorization: `Bearer ${tokenInfo.token}` };

    try {
      setLoading(true);

      await axios.post(
        `${process.env.REACT_APP_API_BASE_URL}/event/create-with-usergid`,
        formData,
        { headers }
      );

      toast.current?.show({
        severity: 'success',
        summary: 'Операція створена',
        detail: 'Операція успішно створена!',
        life: 3000
      });

      setTimeout(() => navigate('/dashboard/operations'), 1000);
    } catch (err: any) {
      const errorResponse = err.response?.data;
      toast.current?.show({
        severity: 'error',
        summary: 'Помилка',
        detail: errorResponse?.message || 'Не вдалося створити операцію',
        life: 4000
      });
    } finally {
      setLoading(false);
    }
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
              <Button label="Створити" onClick={handleSubmit} loading={loading} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}