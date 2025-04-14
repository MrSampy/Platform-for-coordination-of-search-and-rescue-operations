import { useState, useRef } from "react";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { Button } from "primereact/button";
import { classNames } from "primereact/utils";
import { Toast } from "primereact/toast";
import { Link, useNavigate } from "react-router-dom";
import { register } from "../services/authService";
import { RegisterModel } from "../types/authTypes";
import { ErrorModel } from "../types/commonTypes";
import '../styles/auth.css'
const SignupPage = () => {
    const [model, setModel] = useState<RegisterModel>({
        username: "",
        email: "",
        password: ""
    });
    const toast = useRef<Toast>(null);
    const navigate = useNavigate();

    const handleSubmit = async () => {
        try {
            await register(model);
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
        <div className={containerClassName}>
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
                            <span className="text-900 text-3xl font-medium mb-3">Create your account</span>
                        </div>

                        <div>
                            <label htmlFor="username" className="block text-900 text-xl font-medium mb-2">
                                Username
                            </label>
                            <InputText
                                id="username"
                                value={model.username}
                                onChange={(e) => setModel({ ...model, username: e.target.value })}
                                type="text"
                                placeholder="Enter your name"
                                className="w-full md:w-30rem mb-5"
                                style={{ padding: "1rem" }}
                            />

                            <label htmlFor="email" className="block text-900 text-xl font-medium mb-2">
                                Email
                            </label>
                            <InputText
                                id="email"
                                value={model.email}
                                onChange={(e) => setModel({ ...model, email: e.target.value })}
                                type="email"
                                placeholder="Enter your email"
                                className="w-full md:w-30rem mb-5"
                                style={{ padding: "1rem" }}
                            />

                            <label htmlFor="password" className="block text-900 text-xl font-medium mb-2">
                                Password
                            </label>
                            <Password
                                inputId="password"
                                value={model.password}
                                onChange={(e) => setModel({ ...model, password: e.target.value })}
                                placeholder="Enter your password"
                                toggleMask
                                feedback={true}
                                promptLabel="Choose a password"
                                weakLabel="Too simple"
                                mediumLabel="Average complexity"
                                strongLabel="Complex password"
                                className="w-full mb-5"
                                inputClassName="w-full p-3 md:w-30rem"
                            />
                            <label htmlFor="" className="block text-900 text-xl font-medium mb-2">
                            </label>
                            <Button
                                label="Sign Up"
                                onClick={handleSubmit}
                                className="w-full md:w-30rem mb-5"
                                style={{ padding: "1rem" }}
                            />

                            <div className="flex align-items-center justify-content-between mb-5 gap-5">
                                <div className="flex align-items-center">
                                    <label>Already have an account?</label>
                                </div>
                                <Link
                                    to="/"
                                    className="font-medium no-underline ml-2 text-right cursor-pointer"
                                    style={{ color: "var(--primary-color)" }}
                                >
                                    Login
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
