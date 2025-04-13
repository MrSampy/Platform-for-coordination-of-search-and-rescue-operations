import axios from "axios";
import { LoginModel, RegisterModel, TokenInfoDTO, UserDTO } from "../types/authTypes";

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL;

export async function login(model: LoginModel): Promise<TokenInfoDTO> {
  const response = await axios.post<TokenInfoDTO>(`${API_BASE_URL}/token/login`, model);
  return response.data;
}

export async function register(model: RegisterModel): Promise<UserDTO> {
  const response = await axios.post<UserDTO>(`${API_BASE_URL}/authenticate/register`, model);
  return response.data;
}
