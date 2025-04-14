import axios from "axios";
import { LoginModel, RegisterRequest, TokenInfoDTO } from "../types/authTypes";

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL;

export async function login(model: LoginModel): Promise<TokenInfoDTO> {
  const response = await axios.post<TokenInfoDTO>(`${API_BASE_URL}/token/login`, model);
  return response.data;
}

export async function register(model: RegisterRequest): Promise<TokenInfoDTO> {
  const response = await axios.post<TokenInfoDTO>(`${API_BASE_URL}/authenticate/register-worker`, model);
  return response.data;
}
