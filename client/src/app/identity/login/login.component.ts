import {Component, Input, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import ValidateForm from '../helpers/ValidateForm';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  //@Input() isUserLoggedIn: boolean = false;
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  loginForm!: FormGroup;
  isLoggedIn: boolean = false;

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });

    const isLoggedInStr = localStorage.getItem('isLoggedIn');
    if (isLoggedInStr) {
      this.isLoggedIn = isLoggedInStr === 'true';
    }
  }

  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";
  }

  onLogin() {
    if (this.loginForm.valid) {
      this.auth.login(this.loginForm.value).subscribe({
        next: (response) => {
          this.isLoggedIn = true;
          this.auth.saveToken(response.token);
          localStorage.setItem('isLoggedIn', String(this.isLoggedIn));
          this.loginForm.reset();
          this.router.navigateByUrl('/shop');
        },
        error: (error) => {
          alert(error?.error.message);
        }
      });
    } else {
      ValidateForm.validateAllFormFields(this.loginForm);
      alert("Your form is invalid");
    }
  }
}
