
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class TokenService {


  constructor() { }

  getToken(): string | null {
    return localStorage.getItem('token');
    
  }

  decodeToken(): { FullName: string, Email: string, Id: number } | null {
    const token = this.getToken();
    if (token) {
      try {
        console.log(token);
        return jwtDecode(token);

      } catch (error) {
        console.error('Error decoding token:', error);
        return null;
      }
    }
    return null;
  }

  getFullName(): string | null {
    const decodedToken = this.decodeToken();
    console.log(decodedToken);
    return decodedToken ? decodedToken.FullName : null;
    
  }

  getEmail(): string | null {
    const decodedToken = this.decodeToken();
    return decodedToken ? decodedToken.Email : null;
  }

  getUserId(): number | null{
    const decodedToken = this.decodeToken();
    return decodedToken ? decodedToken.Id: null;
  }


  logout() {
    localStorage.removeItem('token');
  }
}
