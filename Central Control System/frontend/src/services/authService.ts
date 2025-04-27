import axios from "axios";
import { LoginModel, TokenInfoDTO, UserDTO, GetTokenRequest, LoginResponse, GetAuthenticatorKeyResponse } from "../types/authTypes";
import { OperationWorkerDTO } from "../types/eventTypes";

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL;

export async function login2fa(model: LoginModel): Promise<LoginResponse> {
  const response = await axios.post<LoginResponse>(`${API_BASE_URL}/authenticate/2fa/login`, model);
  return response.data;
}

export async function getToken2fa(model: GetTokenRequest): Promise<TokenInfoDTO> {
  const response = await axios.post<TokenInfoDTO>(`${API_BASE_URL}/authenticate/2fa/gettoken`, model);
  return response.data;
}

export async function getAuthenticatorKey(model: LoginModel): Promise<GetAuthenticatorKeyResponse> {
  const response = await axios.post<GetAuthenticatorKeyResponse>(`${API_BASE_URL}/authenticate/key`, model);
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