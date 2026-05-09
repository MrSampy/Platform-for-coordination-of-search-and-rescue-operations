import { TokenInfoDTO } from "../types/authTypes";
import Cookies from "js-cookie";
export function logoutAndRedirect() {
    clearAllCookies();
    window.location.href = '/';
}

export function clearAllCookies() {
    const cookies = document.cookie.split(';');
    for (const cookie of cookies) {
      const [name] = cookie.split('=').map(c => c.trim());
      document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; SameSite=Strict; Secure`;
    }
  }

  export function getValidToken(): string {
    const tokenInfoString = Cookies.get('token');
    try {
      if (!tokenInfoString) {
        logoutAndRedirect(); 
        throw new Error('No token found. Redirecting to login.');
      }
    
      let tokenInfo: TokenInfoDTO;
      try {
        tokenInfo = JSON.parse(tokenInfoString);
      } catch {
        logoutAndRedirect(); 
        throw new Error('Invalid token format. Redirecting to login.');
      }
    
      if (new Date(tokenInfo.expiration) < new Date()) {
        logoutAndRedirect(); 
        throw new Error('Token is expired. Redirecting to login.');
      }
    } catch{
      return '';
    }
    return tokenInfoString;
}