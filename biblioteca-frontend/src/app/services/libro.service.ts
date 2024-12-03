import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class LibroService {
  private url = "https://localhost:7299/api/Libros/";

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  obtenerLibros(mostrarActivos:boolean): Observable<any>{
    return this.http.get(`${this.url}?mostrarActivos=${mostrarActivos}`, {headers: this.tokenService.obtenerHeaders()});
  }

  guardarLibro(libro:any): Observable<any>{
    return this.http.post(this.url, libro, {headers: this.tokenService.obtenerHeaders()});
  }

  eliminarLibro(id: Number): Observable<any>{
    return this.http.delete(this.url+id,{headers: this.tokenService.obtenerHeaders()});
  }

  actualizarLibro(libro: any): Observable<any>{
    return this.http.put(this.url, libro, {headers: this.tokenService.obtenerHeaders()});
  }

  // contarLibros(mostrarActivos: boolean): Observable<any>{
  //   return this.http.get(`${this.url}ContarLibros?mostrarActivos=${mostrarActivos}`);
  // }
}
