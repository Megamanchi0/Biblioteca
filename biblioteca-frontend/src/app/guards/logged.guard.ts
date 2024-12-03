import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { TokenService } from '../services/token.service';
import { Router } from '@angular/router';

export const loggedGuard: CanActivateFn = () => {
  const servicioToken = inject(TokenService);
  const router = inject(Router);

  const token = servicioToken.decodificarToken();
  if (!token || servicioToken.estaExpirado()) {
    router.navigate(['/login']);
    return false;
  }
  return true;
};
