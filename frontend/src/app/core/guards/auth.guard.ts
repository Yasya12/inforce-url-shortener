import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Використовуємо сигнал для перевірки
  if (authService.isAuthenticated()) {
    return true;
  }

  // Якщо не залогінений, перенаправляємо на сторінку входу
  return router.createUrlTree(['/login']);
};