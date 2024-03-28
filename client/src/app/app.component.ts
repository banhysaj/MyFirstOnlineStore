import { Component, OnInit } from '@angular/core';
import { AuthService } from './identity/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  isLoggedIn: boolean = false;
  userName: string | null = null;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    const isLoggedInStr = localStorage.getItem('isLoggedIn');
    if (isLoggedInStr) {
      this.isLoggedIn = isLoggedInStr === 'true'; // Convert string to boolean
      if (this.isLoggedIn) {
        this.userName = localStorage.getItem('userName');
      }
    }
  }

  logout(): void {
    this.isLoggedIn = false;
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('userName');
    this.authService.logout(); 
  }
}
