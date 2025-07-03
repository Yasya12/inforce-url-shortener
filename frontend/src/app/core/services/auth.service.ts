import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { LoginResponse } from '../../models/login-response.model';
import { tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { LoginRequest } from '../../models/login-request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  //url
  private apiUrl = `${environment.apiUrl}/auth`;

  //services
  private toastr = inject(ToastrService);
  private http = inject(HttpClient) ;
  private router= inject(Router);

  //states
  isAuthenticated = signal<boolean>(this.hasToken());
  userEmail = signal<string | null>(this.getEmailFromToken()); 

  //methods
  login(loginData: LoginRequest) {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, loginData).pipe(
      tap(response => {
        this.toastr.success('hi', 'Login');

        localStorage.setItem('jwt_token', response.token);
        this.isAuthenticated.set(true);
        this.userEmail.set(this.getEmailFromToken()); 
      })
    );
  }

  logout() {
    return this.http.post(`${this.apiUrl}/logout`, {}).pipe(
      tap(() => {
        this.toastr.success('bye', 'Logout');

        localStorage.removeItem('jwt_token');
        this.isAuthenticated.set(false);
        this.userEmail.set(null); 
        this.router.navigate(['/login']);
      })
    );

  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  private getEmailFromToken(): string | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const decodedToken: { email: string } = jwtDecode(token);
      return decodedToken.email;
    } catch (error) {
      return null;
    }
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('jwt_token');
  }
}