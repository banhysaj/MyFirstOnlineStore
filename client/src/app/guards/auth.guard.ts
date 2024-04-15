import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { AuthService } from '../identity/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard  {

  constructor(private auth: AuthService){

  }
  canActivate(): boolean {
    if(this.auth.isLoggedIn()){
      return true;
    }
    else{
      return false;
    }
  }
  
}
