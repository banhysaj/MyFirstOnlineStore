import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import ValidateForm from '../helpers/ValidateForm';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  signUpForm!: FormGroup;

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router){}

  ngOnInit(): void{
    this.signUpForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]*$/)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)]],
      address: ['', Validators.required],
      phoneNumber: ['', Validators.required]
    })


  }
  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type="text": this.type = "password";
  }

  onSignUp(){
    if(this.signUpForm.valid){
      console.log(this.signUpForm.value);
      this.auth.signUp(this.signUpForm.value).subscribe({
        next: (response =>{
          alert(response.message);
          this.signUpForm.reset();
          this.router.navigate(['login']);
        }),
        error:(error =>{
          alert(error?.error.message)
        })
      })

    }
    else{
      ValidateForm.validateAllFormFields(this.signUpForm)
    }
  }

}
