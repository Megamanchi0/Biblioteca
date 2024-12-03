import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccionService {
  url: string = "https://localhost:7299/api/Acciones";

  constructor(private http: HttpClient) { }

  consultarAcciones(): Observable<any>{
    return this.http.get(this.url);
  }
}
