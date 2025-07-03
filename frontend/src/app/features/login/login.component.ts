import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { LoginRequest } from '../../models/login-request.model';
import { Toast, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  //services
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private toastr = inject(ToastrService);

  //states
  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });
  errorMessage: string | null = null;

  //methods
  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }

    const loginData: LoginRequest = {
      email: this.loginForm.get('email')?.value ?? '',
      password: this.loginForm.get('password')?.value ?? ''
    };

    this.authService.login(loginData).subscribe({
      next: () => {
        this.errorMessage = null;
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.toastr.error('Login failed. Please try again.')
        console.error('Login failed', err);
      }
    });
  }
}