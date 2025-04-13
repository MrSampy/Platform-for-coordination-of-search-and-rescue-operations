import React, { useState, useRef } from "react";
import { Link } from "react-router-dom";
import { Password } from "primereact/password";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { login } from "../services/authService";
import { LoginModel } from "../types/authTypes";
import { useNavigate } from "react-router-dom";
import { ErrorModel } from "../types/commonTypes";
import { Toast } from "primereact/toast";

export default function Login() {
  const [model, setModel] = useState<LoginModel>({ username: "", password: "" });
  const navigate = useNavigate();
  const toast = useRef<Toast>(null);
  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const tokenInfo = await login(model);
      localStorage.setItem("token", tokenInfo.token);
      navigate("/dashboard");
    } catch (err: any) {
      const apiError = err.response?.data as ErrorModel;
      toast.current?.show({
        severity: "error",
        summary: "Login Failed",
        detail: apiError?.message || "An error occurred",
        life: 3000
      });
    }
  };

  return (
    <div className="wrapper signIn">
      <Toast ref={toast} />
      <div className="form">
        <div className="heading">LOGIN</div>
        <form onSubmit={handleSubmit}>
          <div className="authFormInputs">
            <InputText
              id="name"
              value={model.username}
              onChange={(e) => setModel({ ...model, username: e.target.value })}
              placeholder="Enter your name"
              className="w-full"
            />
          </div>

          <div className="authFormInputs">
            <Password
              id="password"
              value={model.password}
              onChange={(e) => setModel({ ...model, password: e.target.value })}
              placeholder="Enter your password"
              toggleMask
              className="w-full authFormPassword"
              inputClassName="w-full"
              feedback={true}
              promptLabel="Choose a password" weakLabel="Too simple" mediumLabel="Average complexity" strongLabel="Complex password"
            />
          </div>

          <Button label="Submit" type="submit" className="w-full mt-4"  />
        </form>

        <p>
          Don't have an account? <Link to="/signup" className="p-0">Sign Up</Link>
        </p>
      </div>
    </div>
  );
}
