import { Nullable } from "./commonTypes";

export type LoginModel = {
    username: string;
    password: string;
  };
  
  export type RegisterModel = {
    username: string;
    email: string;
    password: string;
  };
  
  export interface TokenInfoDTO {
    token: string;
    expiration: string;
  }
  
  export interface RoleDTO {
    id: string;
    name: string;
  }
  
  export interface RoleDetails{
    name: string;
    caption: string;
  }

  export interface UserDTO {
    id: string;
    name: string;
    email: string;
    roles: RoleDTO[];
  }
  export interface RegisterRequest {
    username: string;
    email: string;
    password: string;
    name: string;
    surname: string;
    secondName: string;
    identificationCode: string;
    birthDate: Nullable<Date>;
    role: string;
  }
  