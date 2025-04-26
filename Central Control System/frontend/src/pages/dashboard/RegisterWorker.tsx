import { useState, useRef } from "react";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { Button } from "primereact/button";
import { Toast } from "primereact/toast";
import { Calendar } from "primereact/calendar";
import { RegisterRequest, TokenInfoDTO } from "../../types/authTypes";
import { ErrorModel } from "../../types/commonTypes";
import axios from 'axios';
import { Roles } from "../../types/constants";
import { Dropdown } from 'primereact/dropdown';

export default function SignupPage() {
  const [model, setModel] = useState<RegisterRequest>({
    username: "",
    email: "",
    password: "",
    name: "",
    surname: "",
    secondName: "",
    identificationCode: "",
    birthDate: null,
    role: ""
  });

  const [errors, setErrors] = useState<{ [key: string]: boolean }>({});
  const [loading, setLoading] = useState(false);
  const toast = useRef<Toast>(null);

  const validateFields = (): boolean => {
    const newErrors: { [key: string]: boolean } = {};

    if (!model.username.trim()) newErrors.username = true;
    if (!model.email.trim() || !/\S+@\S+\.\S+/.test(model.email)) newErrors.email = true;
    if (!model.password.trim() || model.password.length < 6) newErrors.password = true;
    if (!model.name.trim()) newErrors.name = true;
    if (!model.surname.trim()) newErrors.surname = true;
    if (!model.secondName.trim()) newErrors.secondName = true;
    if (!model.identificationCode.trim() || model.identificationCode.length !== 10) newErrors.identificationCode = true;
    if (!model.birthDate) newErrors.birthDate = true;
    if (!model.role.trim()) newErrors.role = true;

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validateFields()) {
      toast.current?.show({
        severity: "error",
        summary: "Помилка",
        detail: "Перевірте правильність заповнення всіх полів",
        life: 3000
      });
      return;
    }

    try {
      setLoading(true);
      const tokenInfo = localStorage.getItem('token');
      if (!tokenInfo) return;
      const token = JSON.parse(tokenInfo) as TokenInfoDTO;
      const headers = { Authorization: `Bearer ${token.token}` };

      await axios.post(`${process.env.REACT_APP_API_BASE_URL}/authenticate/register-worker`, model, { headers });      

      toast.current?.show({ severity: "success", summary: "Успіх", detail: "Користувача створено", life: 3000 });
    } catch (err: any) {
      const apiError = err.response?.data as ErrorModel;
      toast.current?.show({
        severity: "error",
        summary: "Помилка реєстрації користувача",
        detail: apiError?.message || "Сталася помилка",
        life: 3000
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="grid justify-content-center">
      <Toast ref={toast} />
      <div className="col-12 md:col-10 lg:col-8">
        <div className="border-round-xl shadow-2 p-4" style={{ backgroundColor: 'white' }}>
          <h2 className="text-center mb-4">Створення працівника</h2>

          <div className="p-fluid formgrid grid">
            <div className="field col-12 md:col-6">
              <label htmlFor="username">Ім'я працівника</label>
              <InputText id="username" value={model.username} className={errors.username ? 'p-invalid' : ''} onChange={(e) => setModel({ ...model, username: e.target.value })}  />
            </div>

            <div className="field col-12 md:col-6">
              <label htmlFor="email">Email</label>
              <InputText id="email" value={model.email} className={errors.email ? 'p-invalid' : ''} onChange={(e) => setModel({ ...model, email: e.target.value })}  />
            </div>

            <div className="field col-12 md:col-6">
              <label htmlFor="password">Пароль</label>
              <Password
                id="password"
                value={model.password}
                className={errors.password ? 'p-invalid w-full' : 'w-full'}
                onChange={(e) => setModel({ ...model, password: e.target.value })}
                toggleMask
                feedback={true}
              />
            </div>

            <div className="field col-12 md:col-6">
              <label htmlFor="name">Ім'я</label>
              <InputText id="name" value={model.name} className={errors.name ? 'p-invalid' : ''} onChange={(e) => setModel({ ...model, name: e.target.value })} />
            </div>

            <div className="field col-12 md:col-6">
              <label htmlFor="surname">Прізвище</label>
              <InputText id="surname" value={model.surname} className={errors.surname ? 'p-invalid' : ''} onChange={(e) => setModel({ ...model, surname: e.target.value })} />
            </div>

            <div className="field col-12 md:col-6">
              <label htmlFor="secondName">По батькові</label>
              <InputText id="secondName" value={model.secondName} className={errors.secondName ? 'p-invalid' : ''} onChange={(e) => setModel({ ...model, secondName: e.target.value })} />
            </div>

            <div className="field col-12 md:col-6">
              <label htmlFor="identificationCode">Ідентифікаційний код</label>
              <InputText
                id="identificationCode"
                value={model.identificationCode}
                className={errors.identificationCode ? 'p-invalid' : ''}
                onChange={(e) => {
                  const value = e.target.value;
                  if (/^\d{0,10}$/.test(value)) {
                    setModel({ ...model, identificationCode: value });
                  }
                }}
                placeholder="до 10 цифр"
              />
            </div>

            <div className="field col-12 md:col-6">
              <label htmlFor="birthDate">Дата народження</label>
              <Calendar
                id="birthDate"
                value={model.birthDate}
                className={errors.birthDate ? 'p-invalid' : ''}
                showIcon
                readOnlyInput
                dateFormat="yy-mm-dd"
                placeholder="yyyy-mm-dd"
                onChange={(e) => {
                  if (e.value) {
                    const date = e.value;
                    const year = date.getFullYear();
                    const month = String(date.getMonth() + 1).padStart(2, '0');
                    const day = String(date.getDate()).padStart(2, '0');
                    const formatted = `${year}-${month}-${day}T00:00:00.000Z`;
                    setModel({ ...model, birthDate: new Date(formatted) });
                  }
                }}
              />
            </div>

            <div className="field col-12">
              <label>Роль</label>
              <Dropdown
                value={model.role}
                options={Roles}
                optionLabel="caption"
                optionValue="name"
                className={errors.role ? 'p-invalid' : 'w-full'}
                onChange={(e) => setModel({ ...model, role: e.value })}
                placeholder="Оберіть роль"
              />
            </div>

            <div className="field col-12 flex justify-content-center align-items-center mt-3">
              <Button
                label="Створити акаунт"
                icon="pi pi-user-plus"
                className="w-6 md:w-4"
                onClick={handleSubmit}
                loading={loading}
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
