import { Component } from '@angular/core';
import { AuthService } from 'src/app/identity/services/auth.service';
import { TokenService } from 'src/app/identity/services/token.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {
  isLoggedIn: boolean = false;
  firstName: string | null = null;
  userId: number | null = null

  constructor(private authService: AuthService, private tokenService: TokenService) {
    this.isLoggedIn = this.authService.isLoggedIn();
    if (this.isLoggedIn) {
      const fullName = this.tokenService.getFullName();
      this.firstName = fullName ? fullName.split(' ')[0] : null;
      this.userId = this.tokenService.getUserId();
    }
  }

  logout() {
    this.authService.logout();
  }
  

}
