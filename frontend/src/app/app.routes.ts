import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'urls',
    loadComponent: () => import('./features/urls-table/urls-table.component').then(m => m.UrlsTableComponent)
  },
  {
    path: 'urls/:id',
    loadComponent: () => import('./features/urls-table/url-info/url-info.component').then(m => m.UrlInfoComponent),
    canActivate: [authGuard]
  },
  {
    path: 'logout',
    loadComponent: () => import('./features/logout/logout.component').then(m => m.LogoutComponent)
  },
  { path: '', redirectTo: '/urls', pathMatch: 'full' },
  { path: '**', redirectTo: '/urls' }
];
