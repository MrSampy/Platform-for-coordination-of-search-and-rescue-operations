import { useState, useRef } from "react";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { Button } from "primereact/button";
import { classNames } from "primereact/utils";
import { Toast } from "primereact/toast";
import { Link, useNavigate } from "react-router-dom";
import { register, getUserByName } from "../services/authService";
import { RegisterRequest } from "../types/authTypes";
import { ErrorModel } from "../types/commonTypes";
import { Calendar } from "primereact/calendar";
import '../styles/auth.css'
const SignupPage = () => {   
    const [model, setModel] = useState<RegisterRequest>({
        username: "",
        email: "",
        password: "",
        name: "",
        surname: "",
        secondName: "",
        identificationCode: "",
        birthDate: null,
    });
    const toast = useRef<Toast>(null);
    const navigate = useNavigate();

    const handleSubmit = async () => {
        try {
            const tokenInfo = await register(model);
            localStorage.setItem("token", tokenInfo.token);
            const user = await getUserByName(model.username);
            localStorage.setItem("user", JSON.stringify(user));
            navigate("/dashboard");
        } catch (err: any) {
            const apiError = err.response?.data as ErrorModel;
            toast.current?.show({
                severity: "error",
                summary: "Registration Failed",
                detail: apiError?.message || "An error occurred",
                life: 3000
            });
        }
    };

    const containerClassName = classNames(
        "surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden",
        { "p-input-filled": true }
    );

    return (
        <div className={containerClassName} style={{ padding: '2rem' }}>
            <Toast ref={toast} />
            <div className="flex flex-column align-items-center justify-content-center authForm">
                <div
                    style={{
                        borderRadius: "56px",
                        padding: "0.3rem",
                        background: "linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 50%)"
                    }}
                >
                    <div className="w-full surface-card py-8 px-5 sm:px-8" style={{ borderRadius: "53px" }}>
                        <div className="text-center mb-5">
                            <span className="text-900 text-3xl font-medium mb-3">Створіть Ваш акаунт</span>
                        </div>

                        <div>
                            <label htmlFor="username" className="block text-900 text-xl font-medium mb-2">                                
                            </label>
                            <InputText
                                id="username"
                                value={model.username}
                                onChange={(e) => setModel({ ...model, username: e.target.value })}
                                type="text"
                                placeholder="Введіть ваш username"
                                className="w-full md:w-30rem mb-5"
                                style={{ padding: "1rem" }}
                            />

                            <label htmlFor="email" className="block text-900 text-xl font-medium mb-2">
                            </label>
                            <InputText
                                id="email"
                                value={model.email}
                                onChange={(e) => setModel({ ...model, email: e.target.value })}
                                type="email"
                                placeholder="Введіть ваш email"
                                className="w-full md:w-30rem mb-5"
                                style={{ padding: "1rem" }}
                            />

                            <label htmlFor="password" className="block text-900 text-xl font-medium mb-2">
                            </label>
                            <Password
                                inputId="password"
                                value={model.password}
                                onChange={(e) => setModel({ ...model, password: e.target.value })}
                                placeholder="Введіть ваш пароль"
                                toggleMask
                                feedback={true}
                                promptLabel="Choose a password"
                                weakLabel="Too simple"
                                mediumLabel="Average complexity"
                                strongLabel="Complex password"
                                className="w-full mb-5"
                                inputClassName="w-full p-3 md:w-30rem"
                            />

                            <label htmlFor="name" className="block text-900 text-xl font-medium mb-2">
                            </label>
                            <InputText
                                id="name"
                                value={model.name}
                                onChange={(e) => setModel({ ...model, name: e.target.value })}
                                type="text"
                                placeholder="Введіть ваше ім'я"
                                className="w-full md:w-30rem mb-5"
                                style={{ padding: "1rem" }}
                            />

                            <label htmlFor="surname" className="block text-900 text-xl font-medium mb-2">
                            </label>
                            <InputText
                                id="surname"
                                value={model.surname}
                                onChange={(e) => setModel({ ...model, surname: e.target.value })}
                                type="text"
                                placeholder="Введіть вашу фамілію"
                                className="w-full md:w-30rem mb-5"
                                style={{ padding: "1rem" }}
                            />

                            <label htmlFor="secondName" className="block text-900 text-xl font-medium mb-2">
                            </label>
                            <InputText
                                id="secondName"
                                value={model.secondName}
                                onChange={(e) => setModel({ ...model, secondName: e.target.value })}
                                type="text"
                                placeholder="Введіть ваш по-батькові"
                                className="w-full md:w-30rem mb-5"
                                style={{ padding: "1rem" }}
                            />

                            <label htmlFor="identificationCode" className="block text-900 text-xl font-medium mb-2">                                
                            </label>
                            <InputText
                                id="identificationCode"
                                value={model.identificationCode}
                                onChange={(e) => {
                                    const newValue = e.target.value;
                                    if (/^\d{0,10}$/.test(newValue)) {
                                      setModel({ ...model, identificationCode: newValue });
                                    }
                                  }}          
                                type="text"
                                placeholder="Введіть ваш ідентифікаційний код"
                                className="w-full md:w-30rem mb-5"
                                style={{ padding: "1rem" }}
                            />

                            <label htmlFor="birthDate" className="block text-900 text-xl font-medium mb-2">                                
                            </label>
                            <Calendar
                                id="birthDate"
                                value={model.birthDate}
                                showIcon 
                                readOnlyInput
                                dateFormat="yy-mm-dd"
                                onChange={(e) => {
                                    if (e.value) {
                                      const localDate = e.value;
                                      const year = localDate.getFullYear();
                                      const month = String(localDate.getMonth() + 1).padStart(2, '0');
                                      const day = String(localDate.getDate()).padStart(2, '0');
                                  
                                      // перетворюємо в yyyy-MM-dd, без часу
                                      const dateString = `${year}-${month}-${day}T00:00:00.000Z`;
                                  
                                      setModel({ ...model, birthDate: new Date(dateString) }); // так, бо TS очікує Date
                                    }
                                  }}
                                  
                                placeholder="Введіть вашу дату народження"
                                className="w-full md:w-30rem mb-5"
                            />
                            <label htmlFor="" className="block text-900 text-xl font-medium mb-2">
                            </label>
                            <Button
                                label="Реєстрація"
                                onClick={handleSubmit}
                                className="w-full md:w-30rem mb-5"
                                style={{ padding: "1rem" }}
                            />

                            <div className="flex align-items-center justify-content-between mb-5 gap-5">
                                <div className="flex align-items-center">
                                    <label>Вже маєте акаунт?</label>
                                </div>
                                <Link
                                    to="/"
                                    className="font-medium no-underline ml-2 text-right cursor-pointer"
                                    style={{ color: "var(--primary-color)" }}
                                >
                                    Логін
                                </Link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default SignupPage;
