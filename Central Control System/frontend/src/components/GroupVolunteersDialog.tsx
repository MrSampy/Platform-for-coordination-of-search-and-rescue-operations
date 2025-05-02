import { useState, useEffect, useRef } from 'react';
import { Dialog } from 'primereact/dialog';
import { Button } from 'primereact/button';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Toast } from 'primereact/toast';
import { Checkbox } from 'primereact/checkbox';
import { DetailGroup } from '../types/groupTypes';
import { VolunteerDTO } from '../types/volunteerTypes';
import { TokenInfoDTO } from '../types/authTypes';
import { Rating } from 'primereact/rating';
import axios from 'axios';
import { getValidToken } from '../services/commonService';

interface GroupVolunteersDialogProps {
  visible: boolean;
  group: DetailGroup;
  onHide: () => void;
}

export default function GroupVolunteersDialog({ visible, group, onHide }: GroupVolunteersDialogProps) {
  const toast = useRef<Toast>(null);
  const [groupVolunteers, setGroupVolunteers] = useState<VolunteerDTO[]>([]);
  const [freeVolunteers, setFreeVolunteers] = useState<VolunteerDTO[]>([]);
  const [toCreate, setToCreate] = useState<string[]>([]);
  const [toDelete, setToDelete] = useState<string[]>([]);
  const [selectedLeaderGID, setSelectedLeaderGID] = useState<string | null>(null);

  useEffect(() => {
    const loadData = async () => {
      const tokenStr = getValidToken();
      if (!tokenStr || !group) return;

      const token = JSON.parse(tokenStr) as TokenInfoDTO;
      const headers = { Authorization: `Bearer ${token.token}` };

      try {
        const [groupRes, freeRes] = await Promise.all([
          axios.get<VolunteerDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/Volunteer/by-group/${group.gid}`, { headers }),
          axios.post<VolunteerDTO[]>(`${process.env.REACT_APP_API_BASE_URL}/Volunteer/by-event`, {
            eventGID: group.eventGID,
            notInGroup: true
          }, { headers })
        ]);

        setGroupVolunteers(groupRes.data);
        setFreeVolunteers(freeRes.data);
        setSelectedLeaderGID(group.leaderGID ?? null);
        setToCreate([]);
        setToDelete([]);
      } catch (error) {
        console.error('Failed to load volunteers', error);
      }
    };

    if (visible) {
      loadData();
    }
  }, [visible, group]);

  const formatRatingNumber = (volunteer: VolunteerDTO) => {
    return (volunteer.ratingNumber / 100) * 5;
  };
  const addVolunteer = (volunteer: VolunteerDTO) => {
    setFreeVolunteers(prev => prev.filter(v => v.gid !== volunteer.gid));
    setGroupVolunteers(prev => [...prev, volunteer]);

    if (toDelete.includes(volunteer.gid)) {
      setToDelete(prev => prev.filter(gid => gid !== volunteer.gid));
    } else {
      setToCreate(prev => [...prev, volunteer.gid]);
    }
  };

  const removeVolunteer = (volunteer: VolunteerDTO) => {
    setGroupVolunteers(prev => prev.filter(v => v.gid !== volunteer.gid));
    setFreeVolunteers(prev => [...prev, volunteer]);

    if (selectedLeaderGID === volunteer.gid) {
      setSelectedLeaderGID(null);
    }

    if (toCreate.includes(volunteer.gid)) {
      setToCreate(prev => prev.filter(gid => gid !== volunteer.gid));
    } else {
      setToDelete(prev => [...prev, volunteer.gid]);
    }
  };

  const handleLeaderChange = (volunteerGID: string) => {
    if (selectedLeaderGID === volunteerGID) {
        setSelectedLeaderGID(null);
      } else {
        setSelectedLeaderGID(volunteerGID);
      }  
  };

  const handleSaveChanges = async () => {
    const tokenStr = getValidToken();
    if (!tokenStr) return;
    const token = JSON.parse(tokenStr) as TokenInfoDTO;
    const headers = { Authorization: `Bearer ${token.token}` };

    try {
      // Save added and removed volunteers
      await Promise.all([
        ...toCreate.map(volunteerGID =>
          axios.post(`${process.env.REACT_APP_API_BASE_URL}/VolunteersGroups`, {
            volunteerGID,
            groupGID: group.gid
          }, { headers })
        ),
        ...toDelete.map(volunteerGID =>
          axios.request({
            method: 'delete',
            url: `${process.env.REACT_APP_API_BASE_URL}/VolunteersGroups`,
            headers,
            data: {
              volunteerGID,
              groupGID: group.gid
            }
          })
        )
      ]);

      // Save leader if changed
      if (selectedLeaderGID !== group.leaderGID && (groupVolunteers.some(v => v.gid === selectedLeaderGID) || selectedLeaderGID === null)) {
        await axios.put(`${process.env.REACT_APP_API_BASE_URL}/Group`, {
          ...group,
          leaderGID: selectedLeaderGID
        }, { headers });
      }

      toast.current?.show({ severity: 'success', summary: 'Успіх', detail: 'Група оновлена', life: 3000 });
      onHide();
    } catch (error) {
      console.error('Failed to save volunteers or leader', error);
      toast.current?.show({ severity: 'error', summary: 'Помилка', detail: 'Не вдалося зберегти зміни', life: 3000 });
    }finally{
        window.location.reload();
      }
  };

  return (
    <>
      <Toast ref={toast} />

      <Dialog
        header={`Учасники групи: ${group.name}`}
        visible={visible}
        style={{ width: '90vw' }}
        modal
        onHide={onHide}
        footer={
          <div className="flex justify-content-end gap-2">
            <Button label="Зберегти" icon="pi pi-check" onClick={handleSaveChanges} style={{ backgroundColor: "rgb(85, 147, 240)" }} />
            <Button label="Скасувати" icon="pi pi-times" className="p-button-secondary" onClick={onHide} style={{ backgroundColor: "rgb(192, 48, 48)" }} />
          </div>
        }
      >
        <div className="grid">
          <div className="col-6">
            <h4>Волонтери в групі</h4>
            <DataTable value={groupVolunteers} tableStyle={{ minWidth: '100%' }}>
              <Column field="surname" header="Прізвище" />
              <Column field="name" header="Ім'я" />
              <Column field="secondName" header="По батькові" />
              <Column
                header="Лідер"
                body={(rowData: VolunteerDTO) => (
                  <Checkbox
                    checked={selectedLeaderGID === rowData.gid}
                    onChange={() => handleLeaderChange(rowData.gid)}
                  />
                )}
                style={{ width: '4rem', textAlign: 'center' }}
              />
              <Column
                header="Рейтинг"
                body={(rowData: VolunteerDTO) => (
                  <Rating value={formatRatingNumber(rowData)} readOnly cancel={false} />
                )}
                style={{ width: '4rem' }}
              />
              <Column
                header="Видалити"
                body={(rowData: VolunteerDTO) => (
                  <Button icon="pi pi-minus" className="p-button-rounded p-button-danger" onClick={() => removeVolunteer(rowData)} />
                )}
                style={{ width: '4rem' }}
              />
            </DataTable>
          </div>

          <div className="col-6">
            <h4>Вільні волонтери</h4>
            <DataTable value={freeVolunteers} tableStyle={{ minWidth: '100%' }}>
              <Column field="surname" header="Прізвище" />
              <Column field="name" header="Ім'я" />
              <Column field="secondName" header="По батькові" />
              <Column
                header="Рейтинг"
                body={(rowData: VolunteerDTO) => (
                  <Rating value={formatRatingNumber(rowData)} readOnly cancel={false} />
                )}
                style={{ width: '4rem' }}
              />
              <Column
                header="Додати"
                body={(rowData: VolunteerDTO) => (
                  <Button icon="pi pi-plus" className="p-button-rounded p-button-success" onClick={() => addVolunteer(rowData)} />
                )}
                style={{ width: '4rem' }}
              />
            </DataTable>
          </div>
        </div>
      </Dialog>
    </>
  );
}