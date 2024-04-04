import { Component, OnInit } from '@angular/core';
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
          this.router.navigate(['shop']);
          this.auth.saveToken(response.token);
          this.isLoggedIn = true;
          localStorage.setItem('isLoggedIn', String(this.isLoggedIn)); // Convert boolean to string
          this.loginForm.reset();
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
