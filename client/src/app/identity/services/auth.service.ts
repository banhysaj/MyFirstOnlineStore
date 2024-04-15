import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl:string ="https://localhost:5001/api/users/"
  constructor(private http : HttpClient) {}


  signUp(userObj: any){
      return this.http.post<any>(`${this.baseUrl}register`, userObj);
  }

  login(loginObj: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}authenticate`, loginObj);
  }

  saveToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue);
  }
  isLoggedIn(): boolean{
    return !!localStorage.getItem('token')
  }
  logout() {
    localStorage.removeItem('token');
  }

}
