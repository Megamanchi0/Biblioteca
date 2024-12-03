import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { Router } from "@angular/router";
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class PerfilService {
  private url = "https://localhost:7299/api/Perfiles/";

  constructor(private http: HttpClient, private router: Router, private tokenService: TokenService) { }

  registrarse(perfil: any): Observable<any>{
    return this.http.post(`${this.url}Registrarse`, perfil);
  }

  iniciarSesion(perfil:any): Observable<any>{
    return this.http.post(this.url, perfil);
  }

  cerrarSesion():any{
    localStorage.removeItem('token');
    this.router.navigate(['/']);
  }

  isLogged(): any{
    const token = localStorage.getItem('token');
    return token;
  }

  actualizarPerfil(perfil: any): Observable<any>{
    return this.http.put(this.url, perfil, {headers: this.tokenService.obtenerHeaders()});
  }

  consultarPerfil(id: number): Observable<any>{
    return this.http.get(this.url+"?id="+id, {headers: this.tokenService.obtenerHeaders()});
  }
}
