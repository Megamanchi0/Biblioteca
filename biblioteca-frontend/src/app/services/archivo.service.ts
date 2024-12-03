import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { url } from 'inspector';
import { Observable } from 'rxjs';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class ArchivoService {
  private url = 'https://localhost:7299/api/Archivos'; // Cambia esto por la URL de tu endpoint

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  consultarTodosZip():Observable<any> {
    return this.http.get(`${this.url}/ConsultarDocumentosZip`, { responseType: 'blob', headers: this.tokenService.obtenerHeaders() });
  }

  consultarTodos(): Observable<any>{
    return this.http.get(`${this.url}/ConsultarDocumentos`, {headers: this.tokenService.obtenerHeaders()})
  }

  cargarArchivo(archivo: File, idPerfil: Number): Observable<any>{
    const formData = new FormData();
    formData.append('archivo', archivo, archivo.name);
    return this.http.post(`${this.url}?idPerfil=${idPerfil}`, formData, {headers: this.tokenService.obtenerHeaders()});
  }

  eliminarArchivo(idArchivo: number, idPerfil: Number): Observable<any>{
    return this.http.delete(`${this.url}?idArchivo=${idArchivo}&idPerfil=${idPerfil}`, {headers: this.tokenService.obtenerHeaders()});
  }

  descargarArchivo(idArchivo: number, idPerfil: Number): Observable<any>{
    return this.http.get(`${this.url}?idArchivo=${idArchivo}&idPerfil=${idPerfil}`, { responseType: 'blob', headers: this.tokenService.obtenerHeaders() })
  }

  consultarHistorial(): Observable<any>{
    return this.http.get(`${this.url}/ConsultarHistorial`, {headers: this.tokenService.obtenerHeaders()})
  }

}
