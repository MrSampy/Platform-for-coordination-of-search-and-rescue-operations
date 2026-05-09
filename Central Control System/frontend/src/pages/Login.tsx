import { useState, useRef } from 'react';
import { Password } from 'primereact/password';
import { InputText } from 'primereact/inputtext';
import { classNames } from 'primereact/utils';
import { useNavigate } from "react-router-dom";
import { login2fa, getToken2fa, getUserByName, getOperationWorkerByUserGID, getAuthenticatorKey } from "../services/authService";
import { LoginModel } from "../types/authTypes";
import { ErrorModel } from "../types/commonTypes";
import { Toast } from "primereact/toast";
import { Button } from "primereact/button";
import { QRCodeCanvas } from 'qrcode.react';
import '../styles/auth.css';
import Cookies from 'js-cookie';

const LoginPage = () => {
  const [model, setModel] = useState<LoginModel>({ username: "", password: "" });
  const [code, setCode] = useState<string>("");
  const [step, setStep] = useState<1 | 2>(1);
  const [authenticatorKey, setAuthenticatorKey] = useState<string | null>(null); // New
  const navigate = useNavigate();
  const toast = useRef<Toast>(null);

  const handleLogin = async () => {
    try {
      const response = await login2fa(model);
      if (response.isValid) {
        setStep(2);
      } else {
        toast.current?.show({ severity: "error", summary: "Помилка", detail: response.message, life: 3000 });
      }
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

  const handleVerifyCode = async () => {
    try {
      const tokenInfo = await getToken2fa({ username: model.username, code });      
      Cookies.set('token', JSON.stringify(tokenInfo), {
        expires: new Date(tokenInfo.expiration), 
        secure: false, 
        sameSite: 'Strict',
      });

      const user = await getUserByName(model.username);
      Cookies.set('user', JSON.stringify(user), {
        expires: new Date(tokenInfo.expiration), 
        secure: false, 
        sameSite: 'Strict',
      });

      const operationworker = await getOperationWorkerByUserGID(user.id);
      Cookies.set('operationWorker', JSON.stringify(operationworker), {
        expires: new Date(tokenInfo.expiration), 
        secure: false, 
        sameSite: 'Strict',
      });

      navigate("/dashboard");
    } catch (err: any) {
      const apiError = err.response?.data as ErrorModel;
      toast.current?.show({
        severity: "error",
        summary: "Invalid Code",
        detail: apiError?.message || "Невірний код або інша помилка",
        life: 3000
      });
    }
  };

  const handleGetAuthenticatorKey = async () => {
    try {
      const keyData = await getAuthenticatorKey(model);
      setAuthenticatorKey(keyData.authenticatorKey);
    } catch (err: any) {
      const apiError = err.response?.data as ErrorModel;
      toast.current?.show({
        severity: "error",
        summary: "Помилка отримання ключа",
        detail: apiError?.message || "Не вдалося отримати QR-код",
        life: 3000
      });
    }
  };

  const containerClassName = classNames('surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden', { 'p-input-filled': true });

  const buildOtpAuthUrl = (userName: string, secret: string) => {
    const issuer = encodeURIComponent('Platform-for-coordination-of-search-and-rescue-operations');
    const label = encodeURIComponent(userName);
    return `otpauth://totp/${issuer}:${label}?secret=${secret}&issuer=${issuer}`;
  };

  return (
    <div className={containerClassName}>
      <Toast ref={toast} />
      <div className="flex flex-column align-items-center justify-content-center authForm">
        <div
            style={{
                borderRadius: '56px',
                padding: '0.3rem',
                background: 'linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 50%)',
                width: '100%'
            }}
        >
          <div className="flex flex-column align-items-center w-full surface-card py-8 px-5 sm:px-8" style={{ borderRadius: '53px' }}>
            <div className="text-center mb-5">
              <span className="text-900 text-3xl font-medium mb-3">
                {step === 1 ? "Увійдіть, щоб продовжити" : "Підтвердіть код 2FA"}
              </span>
            </div>

            
              {step === 1 ? (
                <>
                  <InputText id="Username1" value={model.username} onChange={(e) => setModel({ ...model, username: e.target.value })} type="text" placeholder="Username" className="w-full md:w-30rem mb-5" style={{ padding: '1rem' }} />
                  <Password inputId="password1" feedback={false} value={model.password} onChange={(e) => setModel({ ...model, password: e.target.value })} placeholder="Password" toggleMask className="w-full mb-5" inputClassName="w-full p-3 md:w-30rem" />
                  <Button label="Далі" onClick={handleLogin} className="w-full md:w-30rem mb-5" style={{ padding: '1rem' }} />
                </>
              ) : (
                <>
                  <InputText id="code" value={code} onChange={(e) => setCode(e.target.value)} placeholder="Введіть код з додатку" className="w-full md:w-30rem mb-5" style={{ padding: '1rem' }} />

                  <div className="flex justify-content-between gap-2">
                    <Button label="Підтвердити" onClick={handleVerifyCode} className="w-full md:w-15rem mb-3" style={{ padding: '1rem' }} />
                    <Button label="Отримати QR-код" onClick={handleGetAuthenticatorKey} className="w-full md:w-15rem mb-3 p-button-help" style={{ padding: '1rem' }} />
                  </div>

                  {authenticatorKey && (
                    <div className="flex flex-column align-items-center mt-3">
                      <h5>Скануйте код в додатку</h5>
                      <QRCodeCanvas value={buildOtpAuthUrl(model.username, authenticatorKey)} size={180} />
                    </div>
                  )}
                </>
              )}        
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
