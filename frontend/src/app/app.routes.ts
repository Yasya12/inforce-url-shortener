import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { UrlsTableComponent } from './features/urls-table/urls-table.component';
import { UrlInfoComponent } from './features/urls-table/url-info/url-info.component';
import { LogoutComponent } from './features/logout/logout.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'urls',
    component: UrlsTableComponent
  },
  {
    path: 'urls/:id',
    component: UrlInfoComponent,
    canActivate: [authGuard] 
  },
  {
    path: 'logout',
    component: LogoutComponent,
  },
  { path: '', redirectTo: '/urls', pathMatch: 'full' },
  { path: '**', redirectTo: '/urls' }
];