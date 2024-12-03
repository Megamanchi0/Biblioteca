import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GeneroService {

  url = "https://localhost:7299/api/Generos"

  obtenerHeaders(){
    const token = localStorage.getItem("token");
    return new HttpHeaders({
      "Authorization": `Bearer ${token}`
    })
  }

  consultarGeneros(): Observable<any>{
    return this.http.get(this.url,{headers: this.obtenerHeaders()});
  }

  constructor(private http: HttpClient) { }

}
