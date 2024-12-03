import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../services/token.service';

export const adminGuard = () => {
  const tokenservice = inject(TokenService);

  const token = tokenservice.decodificarToken();
  const router = inject(Router);
  if (!token || tokenservice.estaExpirado()) {
    router.navigate(['/login']);
    return false
  }
  if (token.role!="Administrador") {
    router.navigate(['/prestamo-libros']);
    return false
  }
  return true
};
