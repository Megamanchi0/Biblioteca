import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { jwtDecode } from "jwt-decode";

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() { }

  decodificarToken(): any{
    const token = localStorage.getItem('token');
    if (token) {
      return jwtDecode(token);  
    }
    return null;
  }

  obtenerHeaders(){
    const token = localStorage.getItem("token");
    return new HttpHeaders({
      "Authorization": `Bearer ${token}`
    })
  }

  estaExpirado(){
    const token = this.decodificarToken();
    if (token.exp*1000 < Date.now()) {
      localStorage.removeItem('token');
      alert("Su sesión ha caducado");
      return true;
    }
    return false
  }
}
