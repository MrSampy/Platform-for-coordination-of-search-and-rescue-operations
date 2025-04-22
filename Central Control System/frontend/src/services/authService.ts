import axios from "axios";
import { LoginModel, RegisterRequest, TokenInfoDTO, UserDTO } from "../types/authTypes";
import { OperationWorkerDTO } from "../types/eventTypes";

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL;

export async function login(model: LoginModel): Promise<TokenInfoDTO> {
  const response = await axios.post<TokenInfoDTO>(`${API_BASE_URL}/token/login`, model);
  return response.data;
}

export async function register(model: RegisterRequest): Promise<TokenInfoDTO> {
  const response = await axios.post<TokenInfoDTO>(`${API_BASE_URL}/authenticate/register-worker`, model);
  return response.data;
}
export async function getUserByName(name: string): Promise<UserDTO> {
  const response = await axios.get<UserDTO>(`${API_BASE_URL}/user/get/byname/${name}`);
  return response.data;
}

export async function getOperationWorkerByUserGID(userGid:string, token:string) {
  const response = await axios.get<OperationWorkerDTO>(`${process.env.REACT_APP_API_BASE_URL}/operationworker/byUserGID/${userGid}`, {
    headers: {
      Authorization: `Bearer ${token}`
    }
  });

  return response.data;
}