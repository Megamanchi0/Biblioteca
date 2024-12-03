import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class PrestamoService {

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  url: string = "https://localhost:7299/api/Prestmaos/";

  solicitarPrestamo(solicitud: any): Observable<any>{
    return this.http.post(this.url, solicitud, {headers: this.tokenService.obtenerHeaders()});
  }

  consultarLibrosPrestamo(id:any): Observable<any>{
    return this.http.get(this.url+"?idPerfil="+id, {headers: this.tokenService.obtenerHeaders()});
  }

  devolverPrestamo(id:any){
    return this.http.delete(this.url+"?idPerfil="+id, {headers: this.tokenService.obtenerHeaders()});

  }

  consultarPrestamos(): Observable<any>{
    return this.http.get(`${this.url}ConsultarPrestamos`, {headers: this.tokenService.obtenerHeaders()});
  }

}
