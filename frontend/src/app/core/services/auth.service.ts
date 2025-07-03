import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { LoginResponse } from '../../models/login-response.model';
import { tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { LoginRequest } from '../../models/login-request.model';
import { User } from '../../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  //url
  private apiUrl = `${environment.apiUrl}/auth`;

  //services
  private toastr = inject(ToastrService);
  private http = inject(HttpClient);
  private router = inject(Router);

  //states
  isAuthenticated = signal<boolean>(this.hasToken());
  user: User | null = null;

  constructor() {
    const decodedUser = this.decodeToken();
    this.user = decodedUser;
    this.isAuthenticated.set(this.hasToken());
  }

  //methods
  login(loginData: LoginRequest) {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, loginData).pipe(
      tap(response => {
        this.toastr.success('hi', 'Login');

        localStorage.setItem('jwt_token', response.token);
        const decodedUser = this.decodeToken();

        this.user = decodedUser;
        this.isAuthenticated.set(true);
      })
    );
  }

  logout() {
    return this.http.post(`${this.apiUrl}/logout`, {}).pipe(
      tap(() => {
        this.toastr.success('bye', 'Logout');

        localStorage.removeItem('jwt_token');
        this.isAuthenticated.set(false);
        this.router.navigate(['/login']);
      })
    );

  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  private decodeToken(): User | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decodedToken: any = jwtDecode(token);

      let roles: string[] = [];

      if (decodedToken.roles) {
        roles = JSON.parse(decodedToken.roles);
      }

      const email = decodedToken.email || "";

      return {
        email,
        roles,
      };
    } catch (error) {
      return null;
    }
  }

  isAdmin(): boolean {
    return this.user?.roles.includes('Admin') ?? false;
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('jwt_token');
  }
}