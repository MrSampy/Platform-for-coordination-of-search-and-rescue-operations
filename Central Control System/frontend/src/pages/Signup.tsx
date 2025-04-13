import React, { useState, useRef } from "react";
import { Link } from "react-router-dom";
import { Password } from "primereact/password";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { useNavigate } from "react-router-dom";
import { register } from "../services/authService";
import { RegisterModel } from "../types/authTypes";
import { ErrorModel } from "../types/commonTypes";
import { Toast } from "primereact/toast";

export default function Signup() {
  const [model, setModel] = useState<RegisterModel>({
    username: "",
    email: "",
    password: "",
  });
  const navigate = useNavigate();
  const toast = useRef<Toast>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
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

  return (
    <div className="wrapper signUp">
      <Toast ref={toast} />
      <div className="form">
        <div className="heading">Create an account</div>
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
            <InputText
              id="email"
              value={model.email}
              onChange={(e) => setModel({ ...model, email: e.target.value })}
              placeholder="Enter your mail"
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

          <Button label="Submit" className="w-full mt-4" type="submit" />
        </form>

        <p>
          Have an account? <Link to="/" className="p-0">Login</Link>
        </p>
      </div>
    </div>
  );
}
