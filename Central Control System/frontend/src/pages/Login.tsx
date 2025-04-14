import { useState, useRef } from 'react';
import { Password } from 'primereact/password';
import { InputText } from 'primereact/inputtext';
import { classNames } from 'primereact/utils';
import { Link } from "react-router-dom";
import { login } from "../services/authService";
import { LoginModel } from "../types/authTypes";
import { useNavigate } from "react-router-dom";
import { ErrorModel } from "../types/commonTypes";
import { Toast } from "primereact/toast";
import { Button } from "primereact/button";
import '../styles/auth.css'

const LoginPage = () => {
  const [model, setModel] = useState<LoginModel>({ username: "", password: "" });
  const navigate = useNavigate();
  const toast = useRef<Toast>(null);
  
  const handleSubmit = async () => {
    try {
      const tokenInfo = await login(model);
      localStorage.setItem("token", tokenInfo.token);
      navigate("/dashboard");
    } catch (err: any) {
      const apiError = err.response?.data as ErrorModel;
      toast.current?.show({
        severity: "error",
        summary: "Login Failed",
        detail: apiError?.message || "Сталася помилка",
        life: 3000
      });
    }
  };
    const containerClassName = classNames('surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden', { 'p-input-filled': true });

    return (
        <div className={containerClassName}>
            <Toast ref={toast} />
            <div className="flex flex-column align-items-center justify-content-center authForm">
                <div
                    style={{
                        borderRadius: '56px',
                        padding: '0.3rem',
                        background: 'linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 50%)'
                    }}
                >
                    <div className="w-full surface-card py-8 px-5 sm:px-8" style={{ borderRadius: '53px' }}>
                        <div className="text-center mb-5">
                            <span className="text-900 text-3xl font-medium mb-3">Увійдіть, щоб продовжити</span>
                        </div>

                        <div>
                            <label htmlFor="Username1" className="block text-900 text-xl font-medium mb-2">
                            </label>
                            <InputText id="Username1" value={model.username} onChange={(e) => setModel({ ...model, username: e.target.value })} type="text" placeholder="Username" className="w-full md:w-30rem mb-5" style={{ padding: '1rem' }} />

                            <label htmlFor="password1" className="block text-900 font-medium text-xl mb-2">
                            </label>
                            <Password inputId="password1" feedback={false} value={model.password} onChange={(e) => setModel({ ...model, password: e.target.value })} placeholder="Password" toggleMask className="w-full mb-5" inputClassName="w-full p-3 md:w-30rem"></Password>
                            <label htmlFor="" className="block text-900 text-xl font-medium mb-2">
                            </label>
                            <Button label="Логін" onClick={handleSubmit} className="w-full md:w-30rem mb-5" style={{ padding: '1rem' }} />

                            <div className="flex align-items-center justify-content-between mb-5 gap-5">
                                <div className="flex align-items-center">
                                    <label htmlFor="rememberme1">Ще не маєте акаунту?</label>
                                </div>
                                <Link to="/signup" className="font-medium no-underline ml-2 text-right cursor-pointer" style={{ color: 'var(--primary-color)' }}>
                                     Реєстрація
                                </Link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default LoginPage;