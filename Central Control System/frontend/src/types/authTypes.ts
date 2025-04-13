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
    expiration: string; // або Date
  }
  
  export interface RoleDTO {
    id: string;
    name: string;
  }
  
  export interface UserDTO {
    id: string;
    name: string;
    roles: RoleDTO[];
  }
  